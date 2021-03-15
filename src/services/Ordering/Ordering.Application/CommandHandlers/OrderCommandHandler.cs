namespace Ordering.Application.CommandHandlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using MediatR;

    using Ordering.Application.Commands;
    using Ordering.Domain.AggregateModels;
    using Ordering.DomainService.Interfaces;

    public class OrderCommandHandler :
         IRequestHandler<OrderAddCommandArgs, object>
       , IRequestHandler<OrderUpdateStatusCommandArgs, object>
    {
        private readonly IOrderService service;
        private readonly IMapper mapper;

        public OrderCommandHandler(IOrderService service
            , IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<object> Handle(OrderAddCommandArgs request
            , CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return await Task.FromResult(request.ValidationResult);
            }
            var result = await service.CreateAsync(mapper.Map<Order>(request));
            return await Task.FromResult(result);
        }

        public async Task<object> Handle(OrderUpdateStatusCommandArgs request
           , CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return await Task.FromResult(request.ValidationResult);
            }
            var order = await service.FindAsync(request.Id);
            if (order is null)
            {
                return await Task.FromResult($"request.Id{request.Id} 不存在");
            }
            else
            {
                order.Status = request.Status;
                var result = await service.UpdateAsync(order);
                return await Task.FromResult(result);
            }
        }
    }
}
