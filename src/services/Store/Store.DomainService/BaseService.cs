using AutoMapper;

using DotnetCoreInfra.SeedWork;

using Store.Infrastructure.Repos;

namespace Store.DomainService
{
    public class BaseService
    {
        protected readonly IMapper mapper;
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IStoreRepository repository;

        public BaseService(IStoreRepository repository
            , IMapper mapper
            , IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
    }
}
