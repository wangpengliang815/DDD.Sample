namespace Ordering.Domain.AggregateModels
{
    using System.Collections.Generic;

    using DotNetCore.Infra.Abstractions;
    using DotNetCore.Infra.SeedWork;

    using Ordering.Domain.Enums;

    /// <summary>
    /// 订单实体,聚合根
    /// </summary>
    public class Order
        : BaseDomainEntity, IAggregateRoot
    {
        /// <summary>Gets the total price.</summary>
        /// <value>The total price.</value>
        public decimal TotalPrice { get; set; }

        /// <summary>Gets or sets the status.</summary>
        /// <value>The status.</value>
        public OrderStatus Status { get; set; }

        /// <summary>Gets or sets the detail.</summary>
        /// <value>The detail.</value>
        public List<OrderDetail> Details { get; set; }

        /// <summary>收货相关信息</summary>
        /// <value>The consignee.</value>
        public Consignee Consignee { get; set; }
    }

    /// <summary>值对象</summary>
    public class Consignee
    {
        /// <summary>
        /// 收货人名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string Phone { get; set; }
    }

    /// <summary>子实体</summary>
    /// <seealso cref="DotNetCore.Infra.Abstractions.BaseDomainEntity" />
    public class OrderDetail : BaseDomainEntity
    {
        public string GoodsId { get; set; }

        public string GoodsName { get; set; }

        public int Number { get; set; }
    }
}

