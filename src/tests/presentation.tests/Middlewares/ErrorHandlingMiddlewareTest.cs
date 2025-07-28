using Presentation.Middlewares;
using Domain.Enums;

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

using FluentAssertions;
using Newtonsoft.Json;
using FluentValidation;
using Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public class ErrorHandlerMiddlewareTests
{
    private TestServer CreateServer(RequestDelegate handler)
    {
        var loggerMock = new Mock<ILogger<ErrorHandlerMiddleware>>();

        var builder = new WebHostBuilder()
            .Configure(app =>
            {
                app.UseMiddleware<Presentation.Middlewares.ErrorHandlerMiddleware>(loggerMock.Object);
                app.Run(handler);
            });

        return new TestServer(builder);
    }

    [Fact]
    public async Task Should_Return_BadRequest_For_ApiException()
    {
        var server = CreateServer(context => throw new ApiException(
            ApiExceptionType.Conflict, "API error message"));

        var client = server.CreateClient();
        var response = await client.GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var json = await response.Content.ReadAsStringAsync();
        dynamic obj = JsonConvert.DeserializeObject(json);

        ((string)obj.Type).Should().Be("Conflict");
        ((int)obj.Status).Should().Be(400);
        ((string)obj.Message).Should().Be("API error message");
    }

    [Fact]
    public async Task Should_Return_BadRequest_For_ValidationException()
    {
        var validationFailures = new[]
        {
            new FluentValidation.Results.ValidationFailure("Prop", "Error1"),
            new FluentValidation.Results.ValidationFailure("Prop", "Error2")
        };
        var server = CreateServer(context => throw new ValidationException(validationFailures));

        var client = server.CreateClient();
        var response = await client.GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var json = await response.Content.ReadAsStringAsync();
        dynamic obj = JsonConvert.DeserializeObject(json);

        ((string)obj.Type).Should().Be("ValidationException");
        ((int)obj.Status).Should().Be(400);

        var errorsArray = (Newtonsoft.Json.Linq.JArray)obj.Errors;
        var errors = errorsArray.Select(e => e.ToString()).ToList();
        errors.Should().Contain(new[] { "Error1", "Error2" });
    }

    [Fact]
    public async Task Should_Return_NotFound_For_KeyNotFoundException()
    {
        var server = CreateServer(context => throw new KeyNotFoundException("Not found"));

        var client = server.CreateClient();
        var response = await client.GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var json = await response.Content.ReadAsStringAsync();
        dynamic obj = JsonConvert.DeserializeObject(json);

        ((string)obj.Type).Should().Be("KeyNotFoundException");
        ((int)obj.Status).Should().Be(404);
        ((string)obj.Message).Should().Be("Not found");
    }

    [Fact]
    public async Task Should_Return_InternalServerError_For_UnknownException()
    {
        var server = CreateServer(context => throw new Exception("Unexpected"));

        var client = server.CreateClient();
        var response = await client.GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var json = await response.Content.ReadAsStringAsync();
        dynamic obj = JsonConvert.DeserializeObject(json);

        ((string)obj.Type).Should().Be("InternalServerError");
        ((int)obj.Status).Should().Be(500);
        ((string)obj.Message).Should().Be("Unexpected");
    }
}
