namespace DotnetCoreInfra.DataAccessInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using DotnetCoreInfra.DataAccess;

    /// <summary>
    /// IEntityBatchDataAccessor,实体数据批量操作写访问器接口
    /// </summary>
    public interface IEntityBatchDataAccessor
    {
        Task<int> BatchDeletePhysicallyAsync<TEntity>(Expression<Func<TEntity, bool>> expression
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity;

        Task<List<TEntity>> BatchInsertAsync<TEntity>(List<TEntity> entityList
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity;

        Task<List<TEntity>> BatchLogicDeleteAsync<TEntity>(List<TEntity> entityList
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity;

        Task<List<TEntity>> BatchUpdatePartiallyAsync<TEntity>(List<TEntity> entityList
            , List<string> propertiesToUpdate
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity;
    }
}
