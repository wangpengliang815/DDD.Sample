namespace Store.Domain.AggregateModels
{
    using DotNetCore.Infra.Abstractions;
    using DotNetCore.Infra.SeedWork;

    public class StoreModel
        : BaseDomainEntity, IAggregateRoot
    {
        public string GoodsId { get; set; }

        public string GoodsName { get; set; }

        public string SurplusNumber { get; set; }
    }
}

