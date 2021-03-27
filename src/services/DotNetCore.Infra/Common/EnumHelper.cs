using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCore.Infra.Common
{
    public static class EnumHelper
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<long, string>> descDictionary
            = new ConcurrentDictionary<Type, Dictionary<long, string>>();

        /// <summary>
        /// 获取枚举对应的描述，该方法仅简单的获取描述信息，如未注册过描述信息则直接返回ToString()结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum input)
        {
            Type enumType = input.GetType();
            if (enumType.IsEnum && descDictionary.TryGetValue(enumType, out Dictionary<long, string> dic) && dic != null)
            {
                long key = Convert.ToInt64(input);
                if (dic.ContainsKey(key))
                {
                    return dic[key];
                }
            }
            return input?.ToString();
        }

        /// <summary>
        /// 获取枚举对应的描述，该方法会在当前TEnum尚未注册描述信息时自动注册描述信息
        /// </summary>
        /// <typeparam name="TArrtibute">用于获取描述的Attribute</typeparam>
        /// <param name="input">枚举值</param>
        /// <param name="attrPropName">Attribute中用于获取描述值的public属性</param>
        /// <returns></returns>
        public static string GetDescription<TArrtibute>(this Enum input, string attrPropName = "Description")
            where TArrtibute : Attribute
        {
            Type enumType = input.GetType();
            Type attrType = typeof(TArrtibute);
            RegisterDescription(enumType, attrType, out Dictionary<long, string> dic, attrPropName);
            long key = Convert.ToInt64(input);
            if (dic != null && dic.Count > 0 && dic.ContainsKey(key))
            {
                return dic[key];
            }
            return input?.ToString();
        }

        /// <summary>
        /// 注册枚举的描述，假如已注册过属性描述信息，则该方法将不进行任何处理
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TArrtibute">用于获取描述的Attribute</typeparam>
        /// <param name="attrPropName">Attribute中用于获取描述值的public属性</param>
        public static void RegisterDescription<TEnum, TArrtibute>(string attrPropName = "Description")
            where TArrtibute : Attribute
        {
            Type enumType = typeof(TEnum);
            Type attrType = typeof(TArrtibute);
            RegisterDescription(enumType, attrType, out Dictionary<long, string> dictionary, attrPropName);
        }

        /// <summary>
        /// 注册枚举的描述，注意此处的注册会替换已有的描述枚举（假如有）
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="dictionary">包含描述的字典</param>
        public static void RegisterDescription<TEnum>(IDictionary<TEnum, string> dictionary)
        {
            Type enumType = typeof(TEnum);
            if (enumType.IsEnum)
            {
                Dictionary<long, string> dic = dictionary.ToDictionary(k => Convert.ToInt64(k.Key), v => v.Value);
                descDictionary.AddOrUpdate(enumType, dic, (k, v) => dic);
            }
        }

        private static void RegisterDescription(Type enumType, Type attrType, out Dictionary<long, string> dictionary, string attrPropName)
        {
            dictionary = null;
            //仅枚举时才进行注册
            if (enumType.IsEnum
                && !descDictionary.TryGetValue(enumType, out dictionary))
            {
                Array arrs = Enum.GetValues(enumType);
                dictionary = new Dictionary<long, string>();
                foreach (object e in arrs)
                {
                    string desc = e.ToString();
                    object[] attrs = enumType.GetField(desc).GetCustomAttributes(attrType, false);
                    if (attrs.Length > 0)
                    {
                        System.Reflection.PropertyInfo prop = attrs[0].GetType().GetProperty(attrPropName);
                        if (prop != null)
                        {
                            desc = prop.GetValue(attrs[0]).ToString();
                        }
                    }
                    dictionary.Add(Convert.ToInt64(e), desc);
                }
                descDictionary.TryAdd(enumType, dictionary);
            }
        }
    }
}
