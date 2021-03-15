namespace Ordering.Application.Commands
{
    using System.Collections.Generic;

    using DotnetCoreInfra.Abstractions;

    using Ordering.Domain.AggregateModels;
    using Ordering.Domain.Enums;

    public abstract class OrderCommandArgs : BaseCommandArgs
    {
        public virtual string Id { get; set; }

        public virtual OrderStatus Status { get; set; }

        public decimal TotalPrice { get; set; }

        public Consignee Consignee { get; set; }

        public List<OrderDetail> Details { get; set; }
    }
}
