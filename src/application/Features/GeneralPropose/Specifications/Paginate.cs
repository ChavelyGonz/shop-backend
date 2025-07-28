using Ardalis.Specification;

namespace Application.Features.GeneralPropose.Specifications
{
    /// <summary>
    /// Generic specification to paginate any entity.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class PaginateSpecification<T> : Specification<T> where T : class
    {
        public PaginateSpecification(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            Query.Skip(skip).Take(pageSize);
        }
    }
}
