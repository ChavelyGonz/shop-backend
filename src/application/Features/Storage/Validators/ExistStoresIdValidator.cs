using Application.Features.Storage.Queries;

using FluentValidation;

namespace Application.Features.Storage.Validators
{
    public class ExistStoresIdQueryValidator : AbstractValidator<ExistStoresIdQuery>
    {
        public ExistStoresIdQueryValidator()
        {
        }
    }
}
