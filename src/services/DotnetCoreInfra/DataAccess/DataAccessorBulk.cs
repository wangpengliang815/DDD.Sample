namespace DotnetCoreInfra.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using DotnetCoreInfra.DataAccessInterface;
    using DotnetCoreInfra.Extension;

    using EFCore.BulkExtensions;

    using Microsoft.EntityFrameworkCore;

    /// <summary>基于EFCore.BulkExtensions实现的批量操作</summary>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    /// <seealso cref="RedPI.Data.Five.DataAccess.BaseDataAccessor{TDbContext}" />
    /// <seealso cref="RedPI.Data.DataAccess.IEntityBatchDataAccessor" />
    /// <remarks>
    ///    因为DbContext在事务中无法撤回的原因， 不建议和EntityDataAccessor使用同一个DbContext
    ///    且不支持内存数据库
    /// </remarks>
    public class EntityBulkDataAccessor<TDbContext> :
                    BaseDataAccessor<TDbContext>,
                    IBatchDataAccessor where TDbContext : DbContext
    {
        /// <summary>正常批处理更新的时候， 不允许操作的字段</summary>
        /// <value>The bulk configuration of excluding when editing.</value>
        protected virtual BulkConfig BulkConfigOfExcludingWhenEditing => new BulkConfig
        {
            PropertiesToExclude = AccessorOptions.GetExcludingFieldsWhenEditing(),
            BatchSize = AccessorOptions.BatchSize
        };


        /// <summary>逻辑删除的时候， 只允许更新的字段</summary>
        /// <value>The bulk configuration of including when deleting.</value>
        protected virtual BulkConfig BulkConfigOfIncludingWhenDeleting => new BulkConfig
        {
            PropertiesToInclude = AccessorOptions.GetIncludingFieldsWhenDeleting(),
            BatchSize = AccessorOptions.BatchSize
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDataAccessor{TContext}" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="options">The DataAccessorOptions.</param>
        public EntityBulkDataAccessor(TDbContext dbContext) : base(dbContext, null)
        {
            if (IsMemoryDb(dbContext))
            {
                throw new NotSupportedException("EFCore.Bulk 暂不支持内存数据库,请考虑使用`DataAccessor`！");
            }
        }

        public async Task<List<TEntity>> BatchInsertAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            if (entityList.Count == 0) { return entityList; }

            foreach (TEntity entity in entityList)
            {
                entity.IsDeleted = false;
            }
            await DbContext.BulkInsertAsync(entityList, null).ConfigureAwait(false);
            return entityList;
        }

        public async Task<List<TEntity>> BatchUpdateAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            if (entityList.Count == 0) { return entityList; }

            return await InnerBatchUpdateAsync(entityList
                            , BulkConfigOfExcludingWhenEditing).ConfigureAwait(false);
        }

        public async Task<List<TEntity>> BatchUpdatePartiallyAsync<TEntity>(
            List<TEntity> entityList
             , List<string> propertiesToUpdate
            , CancellationToken cancellationToken = default
            ) where TEntity : BaseEntity
        {
            if (entityList.Count == 0) { return entityList; }

            // 排除Creation&Deletion 字段,加入仅需更新的字段
            BulkConfig config;

            if (propertiesToUpdate != null && propertiesToUpdate.Count > 0)
            {
                config = new BulkConfig
                {
                    PropertiesToInclude = propertiesToUpdate.Concat(AccessorOptions.EditionFields)
                                                            .ToList()
                };
            }
            else
            {
                config = new BulkConfig
                {
                    PropertiesToExclude = AccessorOptions.CreationFields
                                            .Concat(AccessorOptions.DeletionFields)
                                            .ToList()
                };
            }
            return await InnerBatchUpdateAsync(entityList, config)
                                    .ConfigureAwait(false);
        }

        public async Task<List<TEntity>> BatchLogicDeleteAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            foreach (TEntity item in entityList)
            {
                item.IsDeleted = true;
            }
            await InnerBatchUpdateAsync(entityList, BulkConfigOfIncludingWhenDeleting).ConfigureAwait(false);
            return entityList;
        }

        public async Task<int> BatchDeletePhysicallyAsync<TEntity>(
              Expression<Func<TEntity, bool>> expression
            , CancellationToken cancellationToken = default
            ) where TEntity : BaseEntity
        {
            List<TEntity> toBeDeleted = await DbContext.Set<TEntity>()
                                                       .Where(expression)
                                                       .ToListAsync(cancellationToken)
                                                       .ConfigureAwait(false);
            int result = toBeDeleted.Count;
            await DbContext.BulkDeleteAsync(toBeDeleted).ConfigureAwait(false);
            return result;
        }

        private async Task<List<TEntity>> InnerBatchUpdateAsync<TEntity>(
              List<TEntity> entityList
            , BulkConfig config)
            where TEntity : BaseEntity
        {
            foreach (TEntity item in entityList)
            {
                DetachLocal(item);
            }
            await DbContext.BulkUpdateAsync(entityList, config)
                            .ConfigureAwait(false);
            return entityList;
        }
    }
}
