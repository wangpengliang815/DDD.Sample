namespace Ordering.Application.MapperProfiles
{
    using AutoMapper;

    using Ordering.Application.Commands;
    using Ordering.Domain.AggregateModels;

    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            InitOrderProfile();
        }

        private void InitOrderProfile()
        {
            CreateMap<OrderAddCommandArgs, Order>();
        }
    }
}
