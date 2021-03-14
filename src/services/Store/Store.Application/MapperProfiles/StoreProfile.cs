namespace Store.Application.MapperProfiles
{
    using AutoMapper;

    using Store.Application.Commands;
    using Store.Domain.AggregateModels;

    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            InitStoreProfile();
        }

        private void InitStoreProfile()
        {
            CreateMap<AddStoreCommandArgs, StoreModel>();
        }
    }
}
