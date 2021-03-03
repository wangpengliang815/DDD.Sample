using DotnetCoreInfra.SeedWork;

using Microsoft.EntityFrameworkCore;

using Ordering.Domain.AggregateModels;
using Ordering.Infrastructure.Context;

namespace Ordering.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        protected readonly ApplicationDbContext DbContext;

        protected readonly DbSet<TEntity> DbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            DbContext = context;
            DbSet = DbContext.Set<TEntity>();
        }

        public TEntity Create(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            DbSet.Update(entity);
            return entity;
        }
    }
}
