// public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>
// {
//     private readonly IValidator<TRequest> _validator;

//     public ValidationBehavior(IValidator<TRequest> validator)
//     {
//         _validator = validator;
//     }

//     public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
//     {
//         // Validación antes de que el comando sea manejado
//         var validationResult = await _validator.ValidateAsync(request, cancellationToken);
//         if (!validationResult.IsValid)
//         {
//             throw new ValidationException(validationResult.Errors);
//         }

//         // Continuar con el siguiente comportamiento (el manejador)
//         return await next();
//     }
// }


// public class Startup
// {
//     public void ConfigureServices(IServiceCollection services)
//     {
//         // Registrar MediatR
//         services.AddMediatR(Assembly.GetExecutingAssembly());

//         // Registrar el comportamiento de validación (y otros comportamientos)
//         services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

//         // Registrar otros servicios como el repositorio, validadores, etc.
//         services.AddTransient<IProductRepository, ProductRepository>();
//     }
// }




// public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>
// {
//     private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

//     public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
//     {
//         _logger = logger;
//     }

//     public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
//     {
//         _logger.LogInformation($"Handling {typeof(TRequest).Name}");

//         var response = await next();

//         _logger.LogInformation($"Handled {typeof(TResponse).Name}");

//         return response;
//     }
// }


// using MediatR;
// using Microsoft.Extensions.Logging;
// using System.Threading;
// using System.Threading.Tasks;

// public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>
// {
//     private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

//     public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
//     {
//         _logger = logger;
//     }

//     public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
//     {
//         _logger.LogInformation($"Handling {typeof(TRequest).Name}");

//         var response = await next();

//         _logger.LogInformation($"Handled {typeof(TResponse).Name}");

//         return response;
//     }
// }



// using MediatR;
// using Microsoft.Extensions.Logging;
// using System.Threading;
// using System.Threading.Tasks;

// public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>
// {
//     private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

//     public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
//     {
//         _logger = logger;
//     }

//     public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
//     {
//         _logger.LogInformation($"Handling {typeof(TRequest).Name}");

//         var response = await next();

//         _logger.LogInformation($"Handled {typeof(TResponse).Name}");

//         return response;
//     }
// }



// using MediatR;
// using Microsoft.Extensions.Logging;
// using System.Threading;
// using System.Threading.Tasks;

// public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>
// {
//     private readonly IAuthorizationService _authorizationService;
//     private readonly ILogger<AuthorizationBehavior<TRequest, TResponse>> _logger;

//     public AuthorizationBehavior(IAuthorizationService authorizationService, ILogger<AuthorizationBehavior<TRequest, TResponse>> logger)
//     {
//         _authorizationService = authorizationService;
//         _logger = logger;
//     }

//     public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
//     {
//         // Verifica si el usuario tiene permiso para ejecutar la acción
//         var isAuthorized = await _authorizationService.AuthorizeAsync(request);
//         if (!isAuthorized)
//         {
//             _logger.LogWarning("User is not authorized to execute this request.");
//             throw new UnauthorizedAccessException("User is not authorized.");
//         }

//         _logger.LogInformation("User is authorized.");

//         return await next();
//     }
// }



// using MediatR;
// using Microsoft.Extensions.Caching.Memory;
// using System.Threading;
// using System.Threading.Tasks;

// public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>
// {
//     private readonly IMemoryCache _memoryCache;

//     public CacheBehavior(IMemoryCache memoryCache)
//     {
//         _memoryCache = memoryCache;
//     }

//     public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
//     {
//         var cacheKey = $"{typeof(TRequest).Name}-{request.GetHashCode()}";

//         if (_memoryCache.TryGetValue(cacheKey, out TResponse cachedResponse))
//         {
//             // Devuelve la respuesta del caché si existe
//             return cachedResponse;
//         }

//         // Si no está en caché, ejecuta la consulta y guarda el resultado en caché
//         var response = await next();

//         _memoryCache.Set(cacheKey, response, TimeSpan.FromMinutes(30)); // Cache por 30 minutos

//         return response;
//     }
// }


// public class Startup
// {
//     public void ConfigureServices(IServiceCollection services)
//     {
//         // Registrar MediatR
//         services.AddMediatR(Assembly.GetExecutingAssembly());

//         // Registrar los Behaviors
//         services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
//         services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
//         services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
//         services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));

//         // Registrar otros servicios como el repositorio, validadores, etc.
//         services.AddTransient<IProductRepository, ProductRepository>();
//         services.AddTransient<IAuthorizationService, AuthorizationService>();
//         services.AddTransient<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
//         services.AddMemoryCache();
//     }
// }



