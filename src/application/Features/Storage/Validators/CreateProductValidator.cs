
using Domain.Enums;
using Application.Features.Storage.Commands;

using FluentValidation;

namespace Application.Features.Storage.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(o => o.DTO.Name)
                .NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.DTO.Price).GreaterThan(0);
            RuleFor(x => x.DTO.Unit)
                .Must(BeAValidUnit).WithMessage("Invalid unit value.");
        }

        private bool BeAValidUnit(UnitOfMeasurement unit) 
            => Enum.IsDefined(typeof(UnitOfMeasurement), unit);
    }
}


