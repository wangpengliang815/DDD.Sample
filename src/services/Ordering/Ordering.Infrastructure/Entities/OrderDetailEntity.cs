namespace Ordering.Infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    using DotnetCoreInfra.Abstractions;

    [Table("T_Ordering_OrderDetail")]
    public class OrderDetailEntity : BaseEntity
    {
        public string OrderId { get; set; }

        public string GoodsId { get; set; }

        public string GoodsName { get; set; }

        public int Number { get; set; }
    }
}
