using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Domain.Entities
{
    [Table("T_Ordering_Order")]
    public class OrderEntity
    {
        public string Id { get; set; }

        /// <summary>
        /// 收货人名称
        /// </summary>
        public string ConsigneeName { get; private set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string ConsigneePhone { get; private set; }
    }
}
