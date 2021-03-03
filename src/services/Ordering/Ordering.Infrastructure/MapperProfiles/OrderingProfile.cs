using AutoMapper;

using Ordering.Domain.AggregateModels;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.MapperProfiles
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            InitOrderProfile();
        }

        private void InitOrderProfile()
        {
            CreateMap<Order, OrderEntity>();
        }
    }
}
