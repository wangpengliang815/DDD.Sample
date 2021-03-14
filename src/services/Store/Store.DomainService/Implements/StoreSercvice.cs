using System;
using System.Threading.Tasks;

using AutoMapper;

using DotnetCoreInfra.SeedWork;

using Store.Domain.AggregateModels;
using Store.DomainService.Interfaces;
using Store.Infrastructure.Entities;
using Store.Infrastructure.Repos;

namespace Store.DomainService.Implements
{
    public class StoreSercvice : BaseService
        , IStoreService
    {
        public StoreSercvice(IStoreRepository repository
            , IMapper mapper
            , IUnitOfWork unitOfWork) : base(repository, mapper, unitOfWork)
        {
        }

        public async Task<StoreModel> CreateAsync(StoreModel model)
        {
            StoreEntity Store = mapper.Map<StoreEntity>(model);
            Store.Created = DateTime.Now;
            Store.Edited = DateTime.Now;
            Store.Id = Guid.NewGuid().ToString();
            repository.Create(Store);

            await unitOfWork.Commit();
            return mapper.Map<StoreModel>(Store);
        }
    }
}
