namespace DotNetCore.Infra.DataAccessInterface
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using DotNetCore.Infra.Abstractions;

    /// <summary>读写访问器接口</summary>
    public interface IDataAccessor : IBatchDataAccessor
    {
        Task<TEntity> FindAsync<TEntity>(
            string id
            , bool includeDeleted = false) where TEntity : BaseEntity;

        Task<List<TEntity>> GetListAsync<TEntity>(
            bool includeDeleted = false
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity;

        Task<List<TEntity>> GetListAsync<TEntity>(
            Expression<Func<TEntity, bool>> predicate
            , int skip
            , int take
            , bool includeDeleted = false
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity;

        Task<TEntity> InsertAsync<TEntity>(
            TEntity entity
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity;

        Task<TEntity> LogicDeleteAsync<TEntity>(
            string id
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity;

        Task<TEntity> UpdateAsync<TEntity>(
            TEntity entity
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity;

        Task<TEntity> UpdatePartiallyAsync<TEntity>(
            TEntity entity
            , List<string> propertiesToInclude
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity;

        Task<bool> DeletePhysicallyAsync<TEntity>(
            string id
            , CancellationToken cancellationToken = default) where TEntity : BaseEntity;

        /// <summary>执行带返回结果的存储过程</summary>
        /// <typeparam name="TResult">返回集合元素类型</typeparam>
        /// <param name="procedureStatement">存储过程名称（和参数名称）</param>
        /// <param name="parameterDefinitions">存储过程参数定义</param>
        /// <param name="procedureParameters">存储过程的参数</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>存储过程执行过程中的第一个数据集合</returns>
        Task<List<TResult>> ExecProcedureAsync<TResult>(
            string procedureStatement
            , string parameterDefinitions
            , object procedureParameters
            , CancellationToken cancellationToken = default) where TResult : class, new();

        /// <summary>执行存储过程,无返回值</summary>
        /// <param name="procedureStatement">存储过程名称</param>
        /// <param name="parameterDefinitions">存储过程参数定义</param>
        /// <param name="procedureParameters">存储过程参数对象</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task ExecProcedureNoReturnAsync(
            string procedureStatement
            , string parameterDefinitions
            , object procedureParameters
            , CancellationToken cancellationToken = default);

        /// <summary>执行存储过程,无参数,无返回值</summary>
        /// <param name="procedureStatement">存储过程名称</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task ExecProcedureNoReturnAsync(
            string procedureStatement
            , CancellationToken cancellationToken = default);

        /// <summary>执行存储过程,无参数,返回DataSet</summary>
        /// <param name="procedureStatement">The procedure statement.</param>
        /// <param name="parameterDefinitions">The parameter definitions.</param>
        /// <param name="procedureParameters">The procedure parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// Return a dataset, including mutiple datatable result executed out in a procedure. 
        /// A datatable means a 'select' query in the procedure. 
        /// </returns>
        Task<DataSet> ExecProcedureReturnDataSetAsync(
            string procedureStatement
            , string parameterDefinitions
            , object procedureParameters
            , CancellationToken cancellationToken = default);
    }
}
