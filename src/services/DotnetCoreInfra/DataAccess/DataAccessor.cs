namespace DotnetCoreInfra.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using DotnetCoreInfra.DataAccessInterface;
    using DotnetCoreInfra.Exceptions;
    using DotnetCoreInfra.Options;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// 实体数据读写访问器
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="RedPI.Data.DataAccess.IEntityDataAccessor" />
    public partial class DataAccessor<TContext> : BaseDataAccessor<TContext>
        , IDataAccessor
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessor{TContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="options">The options.</param>
        public DataAccessor(TContext dbContext
            , IOptions<DataAccessorOptions> options) : base(dbContext, options)
        {
        }

        public async Task<TEntity> FindAsync<TEntity>(
            Guid id
            , bool includeDeleted = false)
            where TEntity : BaseEntity
        {
            TEntity result = await DbContext.FindAsync<TEntity>(id).ConfigureAwait(false);

            if (result == null)
            {
                return null;
            }
            if (includeDeleted)
            {
                return result;
            }
            if (result.IsDeleted != null && !result.IsDeleted.Value)
            {
                return result;
            }
            return null;
        }

        public async Task<TEntity> InsertAsync<TEntity>(
            TEntity entity
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            if (entity.GetId() == null)
            {
                throw new RequireIdException(typeof(TEntity));
            }

            entity.IsDeleted = false;

            DbContext.Add(entity);
            if (AccessorOptions.SaveImmediately)
            {
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                // 取消新插入对象的跟踪状态
                DbContext.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>(
            bool includeDeleted = false
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>().AsNoTracking();
            if (!includeDeleted)
            {
                query = query.Where(p => p.IsDeleted == false);
            }
            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>(
            Expression<Func<TEntity, bool>> predicate
            , int skip
            , int take
            , bool includeDeleted = false
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>().AsNoTracking()
                            .Where(predicate);
            if (!includeDeleted)
            {
                query = query.Where(p => p.IsDeleted == false);
            }

            return await query.Skip(skip).Take(take)
                  .ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<TEntity> UpdateAsync<TEntity>(
            TEntity entity
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            InternalUpdateAsync(entity);
            List<string> propertiesToExclude =
                AccessorOptions.CreationFields.Concat(AccessorOptions.DeletionFields).ToList();

            return await InnerUpdatePartiallyAsync(entity, null, propertiesToExclude, cancellationToken)
                                 .ConfigureAwait(false);
        }

        private static void InternalUpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity.GetId() == null)
            {
                throw new ArgumentException("Fault: Entity.GetId() is null.", nameof(entity));
            }
        }

        public async Task<TEntity> UpdatePartiallyAsync<TEntity>(
              TEntity entity
            , List<string> propertiesToInclude
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            InternalUpdatePartiallyAsync(entity, propertiesToInclude);
            return await InnerUpdatePartiallyAsync(entity
                        , propertiesToInclude.Concat(AccessorOptions.EditionFields)  // 附加 编辑标记字段
                                            .Except(AccessorOptions.CreationFields)
                                            .Except(AccessorOptions.DeletionFields)  // 排除 创建和删除标记字段
                                            .ToList()
                        , null
                        , cancellationToken)
                 .ConfigureAwait(false);
        }

        private static void InternalUpdatePartiallyAsync<TEntity>(
            TEntity entity
            , List<string> propertiesToInclude) where TEntity : BaseEntity
        {
            if (entity.GetId() == null)
            {
                throw new ArgumentException("Fault: Entity.GetId() is null.", nameof(entity));
            }

            if (propertiesToInclude == null || propertiesToInclude.Count == 0)
            {
                throw new ArgumentException("Fault: propertiesToUpdate must contains at least one property name.", nameof(propertiesToInclude));

            }
        }

        public async Task<TEntity> LogicDeleteAsync<TEntity>(
            Guid id
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            TEntity entity = await FindAsync<TEntity>(id).ConfigureAwait(false);

            if (entity == null)
            {
                return null;
            }

            entity.IsDeleted = true;

            await InnerUpdatePartiallyAsync(entity
                    , AccessorOptions.EditionFields
                            .Concat(AccessorOptions.DeletionFields).ToList()
                    , null
                    , cancellationToken)
                        .ConfigureAwait(false);

            return entity;
        }

        public async Task<bool> DeletePhysicallyAsync<TEntity>(
            Guid id
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            TEntity entity = await FindAsync<TEntity>(id).ConfigureAwait(false);

            if (entity == null)
            {
                return false;
            }

            DbContext.Set<TEntity>().Remove(entity);

            return true;
        }

        private async Task<TEntity> InnerUpdatePartiallyAsync<TEntity>(
               TEntity entity
             , List<string> propertiesToInclude
             , List<string> propertiesToExclude
             , CancellationToken cancellationToken = default)
             where TEntity : BaseEntity
        {
            DetachLocal(entity);
#if debug
            entity.Editor = UserProvider.CurrentUser;
            entity.Edited = UserProvider.Now;
#endif
            InternalInnerUpdatePartiallyAsync(entity, propertiesToInclude, propertiesToExclude);

            if (AccessorOptions.SaveImmediately)
            {
                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            return entity;
        }

        private void InternalInnerUpdatePartiallyAsync<TEntity>(
            TEntity entity
            , List<string> propertiesToInclude
            , List<string> propertiesToExclude) where TEntity : BaseEntity
        {
            EntityEntry<TEntity> entry = DbContext.Entry<TEntity>(entity);

            if (propertiesToInclude != null)
            {
                foreach (string item in propertiesToInclude)
                {
                    // 仅修改部分字段
                    entry.Property(item).IsModified = true;
                }
            }
            else if (propertiesToExclude != null)
            {
                entry.State = EntityState.Modified;
                foreach (string item in propertiesToExclude)
                {
                    // 仅排除部分字段修改
                    entry.Property(item).IsModified = false;
                }
            }
            else
            {
                throw new ArgumentException("Arguments propertiesToInclude and propertiesToExclude cannot be null simultaneously");
            }
        }
    }
}
