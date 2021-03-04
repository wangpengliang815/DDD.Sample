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
        /// <summary>
        /// 收货人名称
        /// </summary>
        public string ConsigneeName { get; private set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string ConsigneePhone { get; private set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalPrice { get; private set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderDate { get; private set; }
    }
}

