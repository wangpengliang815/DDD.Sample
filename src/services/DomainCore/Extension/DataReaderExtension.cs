namespace DomainCore.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// SqlDataReader扩展方法
    /// </summary>
    public static class DataReaderExtension
    {
        /// <summary>
        /// DbDataReader 给与T自动赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T Convert<T>(this DbDataReader reader) where T : class, new()
        {
            T entity = new T();
            PropertyInfo[] proInfoArr = typeof(T).GetProperties();

            List<DataReaderColumns> items = reader.ToColumns();

            items.ForEach((i, item) =>
            {
                PropertyInfo property = proInfoArr.FirstOrDefault(p => p.Name.Equals(item.ColumnName,
                    StringComparison.CurrentCultureIgnoreCase));
                if (property != null && property.CanWrite && item.ColumnValue != DBNull.Value)
                {
                    property.SetValue(entity, item.ColumnValue);
                }
            });
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DbDataReader reader) where T : class, new()
        {
            List<T> source = new List<T>();
            while (reader.Read())
            {
                T item = reader.Convert<T>();
                source.Add(item);
            }
            return source;
        }
        /// <summary>
        /// 将SqlDataReader 转化成列集合
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<DataReaderColumns> ToColumns(this DbDataReader reader)
        {
            List<DataReaderColumns> source = new List<DataReaderColumns>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                DataReaderColumns item = new DataReaderColumns
                {
                    ColumnName = reader.GetName(i),
                    Index = i,
                    ColumnValue = reader[i]
                };

                source.Add(item);
            }
            return source;
        }
    }
    /// <summary>
    /// 将SqlDataReader 转化成列集合
    /// </summary>
    public class DataReaderColumns
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object ColumnValue { get; set; }

        /// <summary>
        /// 列索引
        /// </summary>
        public int Index { get; set; }
    }
}
