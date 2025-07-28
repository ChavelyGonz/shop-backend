
using Domain.Exceptions;
using System.Net;
using Newtonsoft.Json;

namespace Presentation.Middlewares
{
    public class ErrorDto
    {
        public string Type { get; set; }
        public int Status { get; set; }
        public string Message { get; set; } = "None";
        public List<string> Errors { get; set; } = new List<string>();
    }
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {
            try { await _next(context); }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                string result;

                switch (error)
                {
                    case ApiException apiEx:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result = JsonConvert.SerializeObject(new ErrorDto
                        {
                            Type = apiEx.ExceptionType.ToString(),
                            Status = (int)HttpStatusCode.BadRequest,
                            Message = apiEx.Message
                        });
                        break;

                    case FluentValidation.ValidationException validationEx:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        result = JsonConvert.SerializeObject(new ErrorDto
                        {
                            Type = "ValidationException",
                            Status = (int)HttpStatusCode.BadRequest,
                            Errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList()
                        });
                        break;

                    case KeyNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        result = JsonConvert.SerializeObject(new ErrorDto
                        {
                            Type = "KeyNotFoundException",
                            Status = (int)HttpStatusCode.NotFound,
                            Message = error.Message
                        });
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        result = JsonConvert.SerializeObject(new ErrorDto
                        {
                            Type = "InternalServerError",
                            Status = (int)HttpStatusCode.InternalServerError,
                            Message = error.Message
                        });
                        break;
                }
                _logger.LogError(error, "An unhandled exception occurred.");
                await response.WriteAsync(result);
            }
        }
    }
}


