namespace DotnetCoreInfra.DataAccess
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    public abstract class BaseDataAccessor<TContext>
        where TContext : DbContext
    {
        protected DbContext DbContext { get; }

        protected BaseDataAccessor(TContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// 剥离本地跟踪的实体，避免更新的时候提示
        /// The instance of entity type '[TEntity]' 
        ///   cannot be tracked because another instance with the same key value for {'TEntityId'} is already being tracked. 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        protected void DetachLocal<TEntity>(TEntity entity)
                 where TEntity : BaseEntity
        {
            TEntity local = DbContext.Set<TEntity>()
                .Local
                .FirstOrDefault(entry => entry.GetId().Equals(entity.GetId()));
            if (local != null)
            {
                DbContext.Entry(local).State = EntityState.Detached;
            }
        }
    }
}
