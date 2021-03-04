using System;

namespace DotnetCoreInfra.Abstractions
{
    /// <summary>
    /// 定义领域实体基类
    /// </summary>
    public abstract class BaseDomainEntity
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid Id { get; protected set; }
    }
}
