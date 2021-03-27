namespace Store.Infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    using DotNetCore.Infra.Abstractions;

    [Table("T_Store_Store")]
    public class StoreEntity : BaseEntity
    {
        public string GoodsId { get; set; }

        public string GoodsName { get; set; }

        public string SurplusNumber { get; set; }
    }
}
