using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using DotnetCoreInfra.DataAccessInterface;

using Microsoft.EntityFrameworkCore;

namespace DotnetCoreInfra.DataAccess
{
    public partial class DataAccessor<TContext> : IDataAccessor
        where TContext : DbContext
    {
        public async Task<List<TEntity>> BatchInsertAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public async Task<List<TEntity>> BatchUpdateAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public async Task<List<TEntity>> BatchUpdatePartiallyAsync<TEntity>(
            List<TEntity> entityList
            , List<string> propertiesToUpdate
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public async Task<List<TEntity>> BatchLogicDeleteAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public async Task<int> BatchDeletePhysicallyAsync<TEntity>(
            Expression<Func<TEntity, bool>> expression
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }
    }
}
