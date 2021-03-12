using System;
using System.Threading.Tasks;

using AutoMapper;

using DotnetCoreInfra.SeedWork;

using Ordering.Domain.AggregateModels;
using Ordering.Infrastructure.Entities;
using Ordering.DomainService.Interfaces;
using Ordering.Infrastructure.Repos;

namespace Ordering.DomainService.Implements
{
    public class OrderSercvice : BaseService
        , IOrderService
    {
        public OrderSercvice(IOrderRepository orderRepository
            , IMapper mapper
            , IUnitOfWork unitOfWork) : base(orderRepository, mapper, unitOfWork)
        {
        }

        public async Task<Order> CreateAsync(Order model)
        {
            OrderEntity entity = mapper.Map<OrderEntity>(model);
            entity.Id = Guid.NewGuid().ToString();
            orderRepository.Create(entity);
            await unitOfWork.Commit();
            return mapper.Map<Order>(entity);
        }

        public Task<Order> UpdateAsync(Order model)
        {
            throw new NotImplementedException();
        }
    }
}
