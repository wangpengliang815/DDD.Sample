namespace Ordering.Domain.AggregateModels
{
    using System;

    using DotNetCore.Infra.Abstractions;

    /// <summary>子实体</summary>
    /// <seealso cref="DotNetCore.Infra.Abstractions.BaseDomainEntity" />
    public class OrderItem : BaseDomainEntity
    {
        public string ProductId { get; private set; }

        public string ProductName { get; private set; }

        public int Number { get; private set; }

        public Guid OrderId { get; private set; }

        protected OrderItem() { }

        public OrderItem(Guid orderId, string productId, string productName, int number)
        {
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            Number = number;
        }
    }
}
