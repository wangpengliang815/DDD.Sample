namespace DotNetCore.Infra.Abstractions
{
    using System;
    using System.Globalization;

    using DotNetCore.Infra.DataAccessInterface;

    /// <summary>实体基类</summary>
    /// <seealso cref="DotNetCore.Infra.DataAccessInterfaces.IDeletable" />
    public abstract class BaseEntity : IDeletable
    {
        public string Id { get; set; }

        /// <summary>
        /// 信息创建人
        /// </summary> 
        public string Creator { get; set; }

        /// <summary>
        /// 信息创建时间
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string Editor { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime Edited { get; set; }

        /// <summary>逻辑删除标志(0:未删除/1:已删除)</summary>
        /// <value>The is deleted.</value>
        public virtual bool? IsDeleted { get; set; }

        /// <summary>
        /// 返回唯一的40位长度的Id (UnixTime+Guid)
        /// </summary>
        /// <returns></returns>
        public static string NewId()
        {
            string result = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString("x", CultureInfo.CurrentUICulture.NumberFormat)
                        + Guid.NewGuid().ToString("N", CultureInfo.CurrentUICulture.NumberFormat);
            return result;
        }
    }
}
