namespace Ordering.Domain.AggregateModels
{
    using System;
    using System.Collections.Generic;

    using DotNetCore.Infra.Abstractions;
    using DotNetCore.Infra.SeedWork;

    /// <summary>
    /// 订单实体,聚合根
    /// </summary>
    public class Order
        : BaseDomainEntity, IAggregateRoot
    {
        /// <summary>Gets the total price.</summary>
        /// <value>The total price.</value>
        public decimal TotalPrice { get; private set; }

        /// <summary>Gets the order items.</summary>
        /// <value>The order items.</value>
        public List<OrderItem> OrderItems { get; private set; }


        /// <summary>Initializes a new instance of the <see cref="Order"/> class.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="totalPrice">The total price.</param>
        /// <param name="orderItems">The order items.</param>
        public Order(Guid id, decimal totalPrice, List<OrderItem> orderItems) 
        {
            Id = id;
            TotalPrice = totalPrice;
            OrderItems = orderItems;
        }
    }
}

