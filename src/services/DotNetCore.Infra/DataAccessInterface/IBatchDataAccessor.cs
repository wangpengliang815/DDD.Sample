﻿namespace DotNetCore.Infra.DataAccessInterface
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using DotNetCore.Infra.Abstractions;

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
