namespace Ordering.Infrastructure.MapperProfiles
{
    using AutoMapper;

    using Ordering.Domain.AggregateModels;
    using Ordering.Infrastructure.Entities;

    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            InitOrderProfile();
        }

        private void InitOrderProfile()
        {
            CreateMap<Order, OrderEntity>().ReverseMap();
        }
    }
}
