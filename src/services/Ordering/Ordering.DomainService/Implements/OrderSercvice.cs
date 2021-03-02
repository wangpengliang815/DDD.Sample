using System;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using DomainCore.SeedWork;

using Ordering.Domain.AggregateModels;
using Ordering.Domain.Entities;
using Ordering.DomainService.Interfaces;
using Ordering.Infrastructure.Repositories;

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
            entity.OrderId = Guid.NewGuid().ToString();
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
