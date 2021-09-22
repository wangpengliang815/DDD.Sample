namespace DotNetCore.Infra.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using DotNetCore.Infra.Safety;

    /// <summary>
    /// 集合扩展类
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// 将指定集合的元素添加到 System.Collections.Generic.List`1 的末尾。该方法并不会引发异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="s"></param>
        public static void AddCollection<T>(this List<T> source, IEnumerable<T> s)
        {
            if (s.HasAnyElement())
            {
                source.AddRange(s);
            }
        }

        public static void AddCollection<T>(this List<T> source, params T[] s)
        {
            if (s.HasAnyElement())
            {
                source.AddRange(s);
            }
        }

        /// <summary>
        /// 遍历元素,并返回元素索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<int, T> action)
        {
            T[] array = source.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                action(i, array[i]);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T element in enumerable)
            {
                action(element);
            }
        }

        /// <summary>
        /// 判断当前集合对象是否为NULL,是否包含元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool HasAnyElement<T>(this IEnumerable<T> source)
        {
            if (source == null || !source.Any())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断当前集合对象是否 不包含元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool HasNoAnyElement<T>(this IEnumerable<T> source)
        {
            if (source == null || !source.Any())
            {
                return true;
            }

            return false;
        }

        public static string ToQueryString(this Dictionary<string, string> source)
        {
            if (source == null || !source.Any())
            {
                return string.Empty;
            }

            StringBuilder s = new StringBuilder();

            foreach (KeyValuePair<string, string> item in source)
            {
                s.AppendFormat("{0}={1}&", item.Key, item.Value);
            }

            s.Remove(s.Length - 1, 1);
            return s.ToString();
        }

        /// <summary>
        /// 将一个集合,按照给定的数量,分成多个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<List<T>> Partition<T>(this IEnumerable<T> source, int count)
        {
            if (source.HasNoAnyElement())
            {
                return new List<List<T>>();
            }

            if (count <= 0)
            {
                throw new ArgumentException("count", "数量需要大于0");
            }

            List<List<T>> container = new List<List<T>>();

            int take = count;
            int skip = 0;

            while (true)
            {
                List<T> to = source.Skip(skip).Take(take).ToList();
                if (!to.Any())
                {
                    break;
                }

                container.Add(to);
                skip += take;
            }

            return container;
        }

        /// <summary>
        /// 以逗号分隔生成字符串,并加单引号
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToSqlString(this IEnumerable<string> source)
        {
            List<string> t = source.Select(p => string.Format("'{0}'", SqlSecurity.FilterInput(p))).ToList();
            return string.Join(",", t);
        }

        /// <summary>
        /// 以逗号分隔生成字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToSqlString(this IEnumerable<short> source)
        {
            return string.Join(",", source);
        }

        /// <summary>
        /// 以逗号分隔生成字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToSqlString(this IEnumerable<int> source)
        {
            return string.Join(",", source);
        }

        /// <summary>
        /// 以逗号分隔生成字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToSqlString(this IEnumerable<long> source)
        {
            return string.Join(",", source);
        }

        public static bool Contains<T>(this IEnumerable<T> collection, Predicate<T> condition)
        {
            return collection.Any(x => condition(x));
        }
    }
}
