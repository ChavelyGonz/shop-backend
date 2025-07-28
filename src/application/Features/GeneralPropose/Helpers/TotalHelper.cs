using Domain.Repositories;

namespace Application.Features.GeneralPropose.Helpers
{
    public partial class Helper<T> where T : class
    {
        private readonly IEntityReadRepository<T> _repository;
        public Helper(IEntityReadRepository<T> repository)
        {
            _repository = repository;
        }
        public async Task<int> TotalCount(CancellationToken cancellationToken = default) 
            => await _repository.CountAsync(cancellationToken);
    }
}