namespace Ordering.Application.Commands
{
    using System;

    using DomainCore.Commands;

    public abstract class OrderCommandArgs : CommandArgs
    {
        public string ConsigneeName { get; protected set; }

        public string ConsigneePhone { get; protected set; }

        public decimal TotalPrice { get; protected set; }

        public DateTime OrderDate { get; protected set; }
    }
}
