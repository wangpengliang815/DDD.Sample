using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using DotNetCore.Infra.Abstractions;
using DotNetCore.Infra.DataAccessInterface;
using DotNetCore.Infra.Extension;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Infra.DataAccess
{
    public partial class DataAccessor<TContext> : IDataAccessor
        where TContext : DbContext
    {
        public async Task<List<TEntity>> BatchInsertAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            if (entityList.Count == 0) { return entityList; }

            foreach (TEntity entity in entityList)
            {
                entity.IsDeleted = false;
            }
            if (IsMemoryDb(DbContext))
            {
                DbContext.AttachRange(entityList);
                if (AccessorOptions.SaveImmediately)
                {
                    await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    // 取消新插入对象的跟踪状态
                    entityList.ForEach(e => DbContext.Entry(e).State = EntityState.Detached);
                }
            }
            else
            {
                await DbContext.BulkInsertAsync(entityList, null).ConfigureAwait(false);
            }

            return entityList;
        }

        public async Task<List<TEntity>> BatchUpdateAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            if (entityList.Count == 0) { return entityList; }

            return await InnerBatchUpdateAsync(entityList
                            , null
                            , AccessorOptions.GetExcludingFieldsWhenEditing()
                            , cancellationToken)
                                    .ConfigureAwait(false);
        }

        public async Task<List<TEntity>> BatchUpdatePartiallyAsync<TEntity>(
            List<TEntity> entityList
            , List<string> propertiesToUpdate
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            if (entityList.Count == 0) { return entityList; }

            // 排除Creation & Deletion 字段,加入仅需更新的字段
#if debug
            BulkConfig config = new BulkConfig { PropertiesToExclude = creationFields.Concat(deletionFields).ToList() };
#endif
            if (propertiesToUpdate != null && propertiesToUpdate.Count > 0)
            {
                List<string> propertiesToInclude = propertiesToUpdate
                                                .Concat(AccessorOptions.EditionFields)
                                                .Except(AccessorOptions.CreationFields)
                                                .Except(AccessorOptions.DeletionFields)
                                                .ToList();
                return await InnerBatchUpdateAsync(entityList, propertiesToInclude, null, cancellationToken)
                                   .ConfigureAwait(false);
            }
            else
            {
                List<string> propertiesToExclude = AccessorOptions.EditionFields
                                            .Concat(AccessorOptions.DeletionFields)
                                            .ToList();
                return await InnerBatchUpdateAsync(entityList, null, propertiesToExclude, cancellationToken)
                                   .ConfigureAwait(false);
            }
        }

        public async Task<List<TEntity>> BatchLogicDeleteAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            foreach (TEntity item in entityList)
            {
                item.IsDeleted = true;
            }
            await InnerBatchUpdateAsync(entityList
                                , AccessorOptions.GetIncludingFieldsWhenDeleting()
                                , null
                                , cancellationToken)
                                .ConfigureAwait(false);

            return entityList;
        }

        public async Task<int> BatchDeletePhysicallyAsync<TEntity>(
            Expression<Func<TEntity, bool>> expression
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
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
            , List<string> propertiesToInclude
            , List<string> propertiesToExclude
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            InternalInnerBatchUpdateAsync(propertiesToInclude, propertiesToExclude);

            foreach (TEntity item in entityList)
            {
                await InnerUpdatePartiallyAsync(item
                                      , propertiesToInclude
                                      , propertiesToExclude
                                      , cancellationToken).ConfigureAwait(false);
            }
            return entityList;
        }

        private static void InternalInnerBatchUpdateAsync(
            List<string> propertiesToInclude
            , List<string> propertiesToExclude)
        {
            if (propertiesToInclude == null && propertiesToExclude == null)
            {
                throw new ArgumentException("propertiesToInclude,propertiesToExclude 两个属性必须提供且仅仅提供一个非空值");
            }
            if (propertiesToInclude != null && propertiesToExclude != null)
            {
                throw new ArgumentException("propertiesToInclude,propertiesToExclude 两个属性必须提供且仅仅提供一个非空值");
            }
        }
    }
}
