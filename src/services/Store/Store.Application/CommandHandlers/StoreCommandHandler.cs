namespace Store.Application.CommandHandlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using MediatR;

    using Store.Application.Commands;
    using Store.Domain.AggregateModels;
    using Store.DomainService.Interfaces;

    public class StoreCommandHandler :
        IRequestHandler<AddStoreCommandArgs, object>
    {
        private readonly IStoreService StoreService;
        private readonly IMapper mapper;

        public StoreCommandHandler(IStoreService StoreService
            , IMapper mapper)
        {
            this.StoreService = StoreService;
            this.mapper = mapper;
        }

        public async Task<object> Handle(AddStoreCommandArgs request
            , CancellationToken cancellationToken)
        {
            return await Task.FromResult(false);
        }
    }
}
