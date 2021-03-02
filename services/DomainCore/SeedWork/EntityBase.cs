using System;

namespace DomainCore.SeedWork
{
    /// <summary>
    /// 定义领域实体基类
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public Guid Id { get; protected set; }
    }
}
