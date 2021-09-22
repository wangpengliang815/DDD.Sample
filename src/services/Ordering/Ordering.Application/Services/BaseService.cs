namespace Ordering.Application
{
    using AutoMapper;

    using DotNetCore.Infra.SeedWork;

    using Ordering.Domain.AggregateModels;
    using Ordering.Domain.Interfaces;

    public class BaseService
    {
        protected readonly IOrderRepository repository;
        protected readonly IMapper mapper;
        protected readonly IUnitOfWork unitOfWork;

        public BaseService(IOrderRepository repository
            , IMapper mapper
            , IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
    }
}
