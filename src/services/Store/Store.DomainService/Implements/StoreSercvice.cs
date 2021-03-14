
using AutoMapper;

using DotnetCoreInfra.SeedWork;

using Store.DomainService.Interfaces;

namespace Store.DomainService.Implements
{
    public class StoreSercvice : BaseService
        , IStoreService
    {
        public StoreSercvice(IMapper mapper
            , IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }
    }
}
