using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using DotNetCore.Infra.DataAccessInterface;

using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Infra.DataAccess
{
    public partial class DataAccessor<TContext> : IDataAccessor
        where TContext : DbContext
    {
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
            await ExecProcedureNoReturnAsync(procedureStatement, parameterDefinitions
                                                         , BuildSqlParameters(procedureParameters)
                                                         , cancellationToken)
                                                  .ConfigureAwait(false);
        }

        /// <summary>执行存储过程,无参数,无返回值</summary>
        /// <param name="procedureStatement">存储过程名称</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task ExecProcedureNoReturnAsync(
            string procedureStatement
            , CancellationToken cancellationToken = default)
        {
            await ExecProcedureNoReturnAsync(procedureStatement, null, null, cancellationToken)
                       .ConfigureAwait(false);
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
            DbConnection connection = DbContext.Database.GetDbConnection();

            using (DbCommand cmd = connection.CreateCommand())
            {
                DbContext.Database.OpenConnection();

                cmd.CommandText = "sp_executesql";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@stmt", procedureStatement));
                if (parameterDefinitions != null)
                {
                    cmd.Parameters.Add(new SqlParameter("@params", parameterDefinitions));
                    cmd.Parameters.AddRange(procedureParameters);
                }

                await cmd.ExecuteNonQueryAsync(cancellationToken)
                         .ConfigureAwait(false);
            }
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
            SqlParameter[] parameters = BuildSqlParameters(procedureParameters);

            return await ExecProcedureAsync<TResult>(procedureStatement, parameterDefinitions, parameters, cancellationToken).ConfigureAwait(false);
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
            DbConnection connection = DbContext.Database.GetDbConnection();

            using (DbCommand cmd = connection.CreateCommand())
            {
                DbContext.Database.OpenConnection();

                cmd.CommandText = "sp_executesql";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@stmt", procedureStatement));
                cmd.Parameters.Add(new SqlParameter("@params", parameterDefinitions));
                cmd.Parameters.AddRange(procedureParameters);
                DbDataReader dataReader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                System.Collections.ObjectModel.ReadOnlyCollection<DbColumn> columnSchema = dataReader.GetColumnSchema();
                List<TResult> result = new List<TResult>();
                while (dataReader.Read())
                {
                    TResult item = new TResult();
                    Type type = item.GetType();
                    foreach (DbColumn column in columnSchema)
                    {
                        System.Reflection.PropertyInfo propertyInfo =
                                            type.GetProperty(column.ColumnName);
                        if (column.ColumnOrdinal.HasValue
                                    && propertyInfo != null)
                        {
                            //注意需要转换数据库中的DBNull类型
                            object value = dataReader.IsDBNull(column.ColumnOrdinal.Value)
                                                ? null
                                                : dataReader.GetValue(column.ColumnOrdinal.Value);
                            propertyInfo.SetValue(item, value);
                        }
                    }
                    result.Add(item);
                }
                dataReader.Close();
                dataReader.Dispose();

                DbContext.Database.CloseConnection();
                return result;
            }
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
            SqlParameter[] parameters = BuildSqlParameters(procedureParameters);

            return await Task.Run(
                                () => ExecProcedureReturnDataSet(procedureStatement, parameterDefinitions, parameters)
                                , cancellationToken).ConfigureAwait(false);
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
            if (parameterObject == null)
            {
                return Array.Empty<SqlParameter>();
            }
            Type type = parameterObject.GetType();
            List<SqlParameter> result = new List<SqlParameter>();
            foreach (System.Reflection.PropertyInfo p in type.GetProperties())
            {
                SqlParameter sqlParameter = new SqlParameter($"@{p.Name}", p.GetValue(parameterObject));
                result.Add(sqlParameter);
            }

            return result.ToArray();
        }
    }
}
