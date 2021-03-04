namespace DotnetCoreInfra.DataAccessInterface
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using DotnetCoreInfra.DataAccess;

    /// <summary>批量写操作访问器接口</summary>
    public interface IBatchDataAccessor
    {
        Task<int> BatchDeletePhysicallyAsync<TEntity>(
            Expression<Func<TEntity, bool>> expression
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity;

        Task<List<TEntity>> BatchInsertAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity;

        Task<List<TEntity>> BatchLogicDeleteAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity;
        Task<List<TEntity>> BatchUpdateAsync<TEntity>(
          List<TEntity> entityList
          , CancellationToken cancellationToken = default) where TEntity : BaseEntity;

        Task<List<TEntity>> BatchUpdatePartiallyAsync<TEntity>(
            List<TEntity> entityList
            , List<string> propertiesToUpdate
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity;
    }
}
