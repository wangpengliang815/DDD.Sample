using System;

namespace DotNetCore.Infra.Safety
{
    /// <summary>
    /// 对属性进行加密与解密操作
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyEncryAttribute : Attribute
    {
        public PropertyEncryAttribute()
        {

        }
    }
}
