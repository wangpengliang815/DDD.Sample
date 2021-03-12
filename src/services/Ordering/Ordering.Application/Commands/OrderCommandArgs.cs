namespace Ordering.Application.Commands
{
    using System.ComponentModel.DataAnnotations;

    using DotnetCoreInfra.Abstractions;

    public abstract class OrderCommandArgs : BaseCommandArgs
    {
        public virtual string Guid { get; set; }

        public decimal TotalPrice { get; set; }

        public ConsigneeArgs Consignee { get; set; }
    }

    public class ConsigneeArgs
    {
        [Required]
        public string ConsigneeName { get; protected set; }

        [Required]
        public string ConsigneePhone { get; protected set; }
    }
}
