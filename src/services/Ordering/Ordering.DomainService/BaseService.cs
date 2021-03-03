using AutoMapper;

using DotnetCoreInfra.SeedWork;

using Ordering.Domain.AggregateModels;

namespace Ordering.DomainService
{
    public class BaseService
    {
        protected readonly IOrderRepository orderRepository;
        protected readonly IMapper mapper;
        protected readonly IUnitOfWork unitOfWork;

        public BaseService(IOrderRepository orderRepository
            , IMapper mapper
            , IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
    }
}
