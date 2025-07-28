using Domain.Repositories;
using Infrastructure.Persistence.Contexts;

using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class EntityRepository<T> : RepositoryBase<T>, IEntityRepository<T>, IEntityReadRepository<T> where T : class
    {
        public EntityRepository(ShopDbContext dbContext) : base(dbContext) { }
    }
}