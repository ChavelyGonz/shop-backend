// app.UseExceptionHandler(errorApp =>
// {
//     errorApp.Run(async context =>
//     {
//         context.Response.StatusCode = 500;
//         context.Response.ContentType = "application/problem+json";

//         var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
//         var exception = exceptionHandlerPathFeature?.Error;

//         var problem = new ProblemDetails
//         {
//             Title = "An unexpected error occurred.",
//             Status = 500,
//             Detail = exception?.Message
//         };

//         if (exception is AppException appEx)
//         {
//             problem.Extensions["exceptionType"] = appEx.ExceptionType;
//         }

//         await context.Response.WriteAsJsonAsync(problem);
//     });
// });
