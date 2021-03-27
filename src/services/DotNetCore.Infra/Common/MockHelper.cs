using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

using Faker;

namespace DotNetCore.Infra.Common
{
    public static class MockHelper
    {
        /// <summary>
        /// 该方法只针对PrimaryKey是int类型
        /// 如果是string类型的Guid请调整该方法的判断条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool IsPrimaryKey<TEntity>(PropertyInfo property)
             where TEntity : new()
        {
            bool result = false;
            if (property.GetCustomAttribute(typeof(KeyAttribute), true) != null
               || property.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)
               || property.Name.Equals(new TEntity().GetType().Name + "Id", StringComparison.OrdinalIgnoreCase))
            {
                result = true;
            }
            return result;
        }

        private static TEntity BuildEntity<TEntity, T>(Func<T> newId) where TEntity : new()
        {
            TEntity entity = new TEntity();
            PropertyInfo[] properties = entity.GetType().GetProperties();
            foreach (PropertyInfo item in properties)
            {
                if (!item.CanWrite)
                {
                    continue;
                }

                // 排除主键
                if (!IsPrimaryKey<TEntity>(item))
                {
                    SetMockData(entity, item);
                }
                else
                {
                    item.SetValue(entity, newId());
                }
            }
            return entity;
        }

        private static TModel BuildModel<TModel>(Action<TModel> setProperties) where TModel : new()
        {
            TModel result = new TModel();
            PropertyInfo[] properties = result.GetType().GetProperties();
            foreach (PropertyInfo item in properties)
            {
                SetMockData(result, item);
            }

            setProperties?.Invoke(result);

            return result;
        }

        private static void SetMockData(object entity, PropertyInfo property, bool throwException = false)
        {
            try
            {
                // 真实类型,Nullable<T>里的T
                Type columnValueType = GetPropertyType(property);
                if (columnValueType == typeof(DateTime))
                {
                    property.SetValue(entity, DateTime.Now);
                }
                else if (columnValueType == typeof(int) || columnValueType == typeof(short))
                {
                    property.SetValue(entity, Convert.ChangeType(RandomNumber.Next(100), columnValueType));
                }
                else if (columnValueType == typeof(long))
                {
                    property.SetValue(entity, Convert.ChangeType(RandomNumber.Next(100) * 100000000, columnValueType));
                }
                else if (columnValueType == typeof(decimal))
                {
                    property.SetValue(entity, Convert.ChangeType(RandomNumber.Next(100000) / 99.0, columnValueType));
                }
                else if (columnValueType == typeof(float) || columnValueType == typeof(double))
                {
                    property.SetValue(entity, RandomNumber.Next(100000) / 99.0);
                }
                else if (columnValueType == typeof(bool))
                {
                    property.SetValue(entity, RandomNumber.Next(10) % 2 == 0);
                }
                else if (property.PropertyType == typeof(string))
                {
                    property.SetValue(entity, Name.FullName());
                }
                else if (columnValueType.IsEnum)
                {
                    Array availableValues = Enum.GetValues(columnValueType);
                    object value = availableValues.GetValue(
                               RandomNumber.Next(availableValues.Length));
                    property.SetValue(entity, value);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw new ArgumentException($"无法设置属性{property.Name}的值,类型为{property.PropertyType}", ex);
                }
            }
        }

        private static Type GetPropertyType(PropertyInfo info)
        {

            if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                return info.PropertyType.GetGenericArguments()[0];
            }
            return info.PropertyType;
        }

        public static TEntity MockEntity<TEntity, T>(Func<T> newId)
          where TEntity : new()
        {
            return BuildEntity<TEntity, T>(newId);
        }

        public static TEntity MockEntity<TEntity>()
          where TEntity : new()
        {
            return BuildEntity<TEntity, string>(() => NewUniqueId());
        }

        /// <summary>
        /// 返回唯一的40位长度的Id (UnixTime+Guid)
        /// </summary>
        /// <returns></returns>
        private static string NewUniqueId()
        {
            string result = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString("x", CultureInfo.CurrentUICulture.NumberFormat)
                        + Guid.NewGuid().ToString("N", CultureInfo.CurrentUICulture.NumberFormat);
            return result;
        }

        public static TModel MockModel<TModel>(Action<TModel> setProperties = null)
          where TModel : new()
        {
            return BuildModel<TModel>(setProperties);
        }

        public static List<TEntity> MockDataList<TEntity, T>(Func<T> newId, int count = 100)
           where TEntity : new()
        {
            List<TEntity> list = new List<TEntity>();


            for (int i = 0; i < count; i++)
            {
                TEntity entity = MockEntity<TEntity, T>(newId);
                list.Add(entity);
            }
            return list;
        }

        public static List<TEntity> MockDataList<TEntity>(int count = 100)
           where TEntity : new()
        {
            List<TEntity> list = new List<TEntity>();

            for (int i = 0; i < count; i++)
            {
                TEntity entity = MockEntity<TEntity, string>(() => NewUniqueId());
                list.Add(entity);
            }
            return list;
        }
    }
}
