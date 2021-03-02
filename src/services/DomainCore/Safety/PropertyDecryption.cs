using System;
using System.Linq;
using System.Reflection;

namespace DomainCore.Safety
{
    public static class PropertyDecryption
    {
        private static readonly string Seed = "C@/?";
        private static readonly SymmetricCrypto password = new SymmetricCrypto(CryptoMode.DES) { Seed = Seed };
        private static readonly Type PropertyEncryType = typeof(PropertyEncryAttribute);

        /// <summary>
        /// 实体属性标有PropertyEncryAttribute进行加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static void Ecryption<T>(T t) where T : class
        {
            PropertyInfo[] properties = typeof(T).GetProperties().Where(p => p.GetCustomAttribute(PropertyEncryType) != null && p.CanWrite).ToArray();
            foreach (PropertyInfo item in properties)
            {
                object value = item.GetValue(t);
                string f = password.EncryptString(value.ToString());
                item.SetValue(t, f);
            }
        }

        /// <summary>
        /// 实体属性标有PropertyEncryAttribute进行解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static void Decryption<T>(T t) where T : class
        {
            PropertyInfo[] properties = typeof(T).GetProperties().Where(p => p.GetCustomAttribute(PropertyEncryType) != null && p.CanWrite).ToArray();
            foreach (PropertyInfo item in properties)
            {
                object value = item.GetValue(t);
                item.SetValue(t, password.DecryptString(value.ToString()));
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Ecryption(string value)
        {
            string f = password.EncryptString(value.ToString());
            return f;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Decryption(string value)
        {
            return password.DecryptString(value.ToString());
        }
    }
}
