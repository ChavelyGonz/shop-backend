using Ardalis.Specification;

namespace Domain.Repositories
{
    public interface IEntityRepository<T> : IRepositoryBase<T> where T : class { }
    public interface IEntityReadRepository<T> : IReadRepositoryBase<T> where T : class { }
}