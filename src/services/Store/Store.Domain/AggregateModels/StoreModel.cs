namespace Store.Domain.AggregateModels
{
    using DotnetCoreInfra.Abstractions;
    using DotnetCoreInfra.SeedWork;

    public class StoreModel
        : BaseDomainEntity, IAggregateRoot
    {
        public string GoodsId { get; set; }

        public string GoodsName { get; set; }

        public string SurplusNumber { get; set; }
    }
}

