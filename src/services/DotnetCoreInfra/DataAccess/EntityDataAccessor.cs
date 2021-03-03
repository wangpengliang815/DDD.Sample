namespace DotnetCoreInfra.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using DotnetCoreInfra.DataAccessInterface;
    using DotnetCoreInfra.Exceptions;

    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// 实体数据读写访问器
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="RedPI.Data.DataAccess.IEntityDataAccessor" />
    public partial class EntityDataAccessor<TContext> : BaseDataAccessor<TContext>
        , IEntityDataAccessor
        where TContext : DbContext
    {
        public EntityDataAccessor(TContext dbContext) :
            base(dbContext)
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

            await DbContext.AddAsync(entity);
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
                            .Where<TEntity>(predicate);
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
            throw new NotImplementedException();
        }

        public async Task<TEntity> UpdatePartiallyAsync<TEntity>(
              TEntity entity
            , List<string> propertiesToInclude
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> LogicDeleteAsync<TEntity>(
            TEntity entity
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks>
        ///   只做逻辑删除， 不包含其他任何业务字段的更新
        /// </remarks>
        public async Task<TEntity> LogicDeleteAsync<TEntity>(
            Guid id
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> DeletePhysicallyAsync<TEntity>(
            TEntity entity
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> DeletePhysicallyAsync<TEntity>(
            Guid id
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>执行不带返回值的存储过程</summary>
        /// <param name="procedureStatement">存储过程名称</param>
        /// <param name="procedureParameters">存储过程参数对象</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task ExecProcedureNoReturnAsync(
            string procedureStatement
            , string parameterDefinitions
            , object procedureParameters
            , CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>执行存储过程,  无参数， 无返回值</summary>
        /// <param name="procedureStatement">存储过程名称</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task ExecProcedureNoReturnAsync(
            string procedureStatement
            , CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>执行不带返回值的存储过程</summary>
        /// <param name="procedureStatement">The procedure statement.</param>
        /// <param name="parameterDefinitions">The parameter definitions.</param>
        /// <param name="procedureParameters">The procedure parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task ExecProcedureNoReturnAsync(
            string procedureStatement
            , string parameterDefinitions
            , SqlParameter[] procedureParameters
            , CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>执行带返回结果的存储过程</summary>
        /// <typeparam name="TResult">返回集合元素类型</typeparam>
        /// <param name="procedureStatement">存储过程名称（和参数名称）例： sp_GetName @id</param>
        /// <param name="parameterDefinitions">The parameter definitions. 例 @id it</param>
        /// <param name="procedureParameters">存储过程的参数 例 @id:10 </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>存储过程执行过程中的第一个数据集合</returns>
        public async Task<List<TResult>> ExecProcedureAsync<TResult>(
            string procedureStatement
            , string parameterDefinitions
            , object procedureParameters
            , CancellationToken cancellationToken = default)
            where TResult : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>执行带返回结果的存储过程</summary>
        /// <typeparam name="TResult">返回集合元素类型</typeparam>
        /// <param name="procedureStatement">存储过程名称（和参数名称）</param>
        /// <param name="parameterDefinitions">存储过程参数定义.</param>
        /// <param name="procedureParameters">存储过程的参数值</param>
        /// <returns>存储过程执行过程中的第一个数据集合</returns>
        private async Task<List<TResult>> ExecProcedureAsync<TResult>(
            string procedureStatement
            , string parameterDefinitions
            , SqlParameter[] procedureParameters
            , CancellationToken cancellationToken = default)
            where TResult : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>Executes the procedure return data set asynchronous.</summary>
        /// <param name="procedureStatement">The procedure statement.</param>
        /// <param name="parameterDefinitions">The parameter definitions.</param>
        /// <param name="procedureParameters">The procedure parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<DataSet> ExecProcedureReturnDataSetAsync(
            string procedureStatement
            , string parameterDefinitions
            , object procedureParameters
            , CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>Executes the procedure return data set asynchronous.</summary>
        /// <param name="procedureStatement">The procedure statement.</param>
        /// <param name="parameterDefinitions">The parameter definitions.</param>
        /// <param name="procedureParameters">The procedure parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private DataSet ExecProcedureReturnDataSet(
            string procedureStatement
            , string parameterDefinitions
            , SqlParameter[] procedureParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据对象的属性， 构建SQL命令参数数组
        /// </summary>
        /// <param name="parameterObject"></param>
        /// <returns></returns>
        /// <remarks>
        ///   暂未考虑带out 参数
        /// </remarks>
        private SqlParameter[] BuildSqlParameters(object parameterObject)
        {
            throw new NotImplementedException();
        }

        /// <summary>Batches the insert asynchronously.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityList">The entity list.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">entityList</exception>
        public async Task<List<TEntity>> BatchInsertAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>Batches the update asynchronously.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityList">The entity list.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">entityList</exception>
        public async Task<List<TEntity>> BatchUpdateAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>Batches the update partially asynchronously.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityList">The entity list.</param>
        /// <param name="propertiesToUpdate">The properties to update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">entityList</exception>
        public async Task<List<TEntity>> BatchUpdatePartiallyAsync<TEntity>(
            List<TEntity> entityList
            , List<string> propertiesToUpdate
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 批量逻辑删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks>
        ///   只做逻辑删除， 不包含其他任何业务字段的更新
        /// </remarks>
        public async Task<List<TEntity>> BatchLogicDeleteAsync<TEntity>(
            List<TEntity> entityList
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>Batches the delete physically asynchronous.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<int> BatchDeletePhysicallyAsync<TEntity>(
            Expression<Func<TEntity, bool>> expression
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }

        /// <summary>Batches the update asynchronously.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityList">The entity list.</param>
        /// <param name="propertiesToInclude">The properties to include.</param>
        /// <param name="propertiesToExclude">The properties to exclude.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks>
        /// propertiesToExclude,propertiesToExclude 两个属性必须提供且仅仅提供一个非空值
        /// </remarks>
        private async Task<List<TEntity>> InnerBatchUpdateAsync<TEntity>(
              List<TEntity> entityList
            , List<string> propertiesToInclude
            , List<string> propertiesToExclude
            , CancellationToken cancellationToken = default)
            where TEntity : BaseEntity
        {
            throw new NotImplementedException();
        }
    }
}
