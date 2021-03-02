namespace Ordering.Application.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DomainCore.Commands;

    public abstract class OrderCommandArgs : CommandArgs
    {
        [Required]
        public string ConsigneeName { get; protected set; }

        [Required]
        public string ConsigneePhone { get; protected set; }
    }
}
