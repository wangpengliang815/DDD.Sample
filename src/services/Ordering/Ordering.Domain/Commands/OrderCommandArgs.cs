namespace Ordering.Domain.Commands
{
    using System;
    using System.Collections.Generic;

    using DotNetCore.Infra.Abstractions;

    using Ordering.Domain.AggregateModels;

    public abstract class OrderCommandArgs : BaseCommandArgs
    {
        public virtual Guid Id { get; set; }

        public decimal TotalPrice { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
