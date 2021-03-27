namespace DotNetCore.Infra.DataAccess
{

    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;

    using DotNetCore.Infra.DataAccessInterface;
    using DotNetCore.Infra.SeedWork;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;


    /// <summary>使用EntityFramework的UnitOfWork, 可以用来维护多次SaveChange时的事务性。</summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <remarks>
    /// EF SaveChange本身就带原子性事务操作。所以在没有多次SaveChange的情况下，可以不需要额外的UnitOfWork
    /// </remarks>
    public class EfUnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        /// <summary>事务对象</summary>
        private IDbContextTransaction currentTransaction;

        /// <summary>
        /// default : ReadCommitted
        /// </summary>
        private readonly IsolationLevel defaultIsolationLevel = IsolationLevel.ReadCommitted;

        /// <summary>
        /// dbContext
        /// </summary>
        private readonly TContext dbContext;

        public EfUnitOfWork(TContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary> Begins the transaction asynchronously.</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            currentTransaction = currentTransaction ??
                        await dbContext.Database.BeginTransactionAsync(defaultIsolationLevel, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>提交事务</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public void CommitTransaction()
        {
            try
            {
                currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (currentTransaction != null)
                {
                    currentTransaction.Dispose();
                    currentTransaction = null;
                }
            }
        }

        /// <summary>回滚事务</summary>
        /// <returns></returns>
        public void RollbackTransaction()
        {
            try
            {
                currentTransaction?.Rollback();
            }
            finally
            {
                if (currentTransaction != null)
                {
                    currentTransaction.Dispose();
                    currentTransaction = null;
                }
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(CommitTransaction, cancellationToken);
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(RollbackTransaction, cancellationToken);

        }

        /// <summary>
        /// 保存数据(开启事务,但并未提交）
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>true 表示有数据更改</returns>
        public async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
        {
            int value = await dbContext.SaveChangesAsync(cancellationToken)
                              .ConfigureAwait(false);

            return value > 0;
        }

        public Task<bool> Commit()
        {
            throw new System.NotImplementedException();
        }
    }
}
