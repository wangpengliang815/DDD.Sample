//namespace Ordering.Application.Implements
//{
//    using System;
//    using System.Threading.Tasks;

//    using AutoMapper;

//    using DotNetCore.Infra.SeedWork;

//    using Ordering.Application.Interfaces;
//    using Ordering.Domain.AggregateModels;
//    using Ordering.Domain.Enums;

//    public class OrderSercvice : BaseService
//        , IOrderService
//    {
//        public OrderSercvice(IOrderRepository orderRepository
//            , IMapper mapper
//            , IUnitOfWork unitOfWork) : base(orderRepository, mapper, unitOfWork)
//        {
//        }

//        public async Task<Order> AddAsync(Order model)
//        {
//            OrderEntity order = mapper.Map<OrderEntity>(model);
//            order.Created = DateTime.Now;
//            order.Edited = DateTime.Now;
//            order.Id = Guid.NewGuid().ToString();
//            order.Status = OrderStatus.Inactived;
//            repository.Create(order);

//            foreach (var item in model.Details)
//            {
//                OrderDetailEntity orderDetail = new OrderDetailEntity
//                {
//                    Id = Guid.NewGuid().ToString(),
//                    OrderId = order.Id,
//                    GoodsId = item.GoodsId,
//                    GoodsName = item.GoodsName,
//                    Number = item.Number,
//                    Created = DateTime.Now,
//                    Edited = DateTime.Now
//                };
//                repository.CreateDetail(orderDetail);
//            }
//            await unitOfWork.Commit();
//            return mapper.Map<Order>(order);
//        }

//        public async Task<Order> GetAsync(string id)
//        {
//            var result = await repository.FindAsync(id);
//            return mapper.Map<Order>(result);
//        }

//        public async Task<Order> UpdateAsync(Order model)
//        {
//            OrderEntity entity = mapper.Map<OrderEntity>(model);
//            entity.Created = DateTime.Now;
//            entity.Edited = DateTime.Now;
//            repository.Update(entity);
//            await unitOfWork.Commit();
//            return model;
//        }
//    }
//}
