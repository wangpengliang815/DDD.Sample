namespace Store.Infrastructure.MapperProfiles
{
    using AutoMapper;

    using Store.Domain.AggregateModels;
    using Store.Infrastructure.Entities;

    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            InitStoreProfile();
        }

        private void InitStoreProfile()
        {
            CreateMap<StoreModel, StoreEntity>().ReverseMap();
        }
    }
}
