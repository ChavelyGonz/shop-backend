
using FluentValidation;
using MediatR;

namespace Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Validación antes de que el comando sea manejado
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Continuar con el siguiente comportamiento (el manejador)
            return await next();
        }
    }
}

