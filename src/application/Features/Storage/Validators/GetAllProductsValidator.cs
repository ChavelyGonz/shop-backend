using FluentValidation;

using Application.Features.Storage.Queries;

namespace Application.Features.Storage.Validators
{
    public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
    {
        public GetAllProductsQueryValidator()
        {
        }
    }
}
