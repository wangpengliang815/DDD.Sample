using System;

using DotnetCoreInfra.Abstractions;
using DotnetCoreInfra.SeedWork;

namespace Ordering.Domain.AggregateModels
{
    /// <summary>
    /// 订单实体,聚合根
    /// </summary>
    public class Order
        : BaseDomainEntity, IAggregateRoot
    {
        /// <summary>Gets the total price.</summary>
        /// <value>The total price.</value>
        public decimal TotalPrice { get; private set; }

        /// <summary>收货相关信息</summary>
        /// <value>The consignee.</value>
        public Consignee Consignee { get; private set; }
    }

    public class Consignee
    {
        /// <summary>
        /// 收货人名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string Phone { get; private set; }
    }
}

