using AutoMapper;

using DotnetCoreInfra.SeedWork;

namespace Store.DomainService
{
    public class BaseService
    {
        protected readonly IMapper mapper;
        protected readonly IUnitOfWork unitOfWork;

        public BaseService(IMapper mapper
            , IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
    }
}
