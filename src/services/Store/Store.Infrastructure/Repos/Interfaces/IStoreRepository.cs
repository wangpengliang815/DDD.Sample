using Store.Infrastructure.Entities;

namespace Store.Infrastructure.Repos
{
    public interface IStoreRepository
    {
        StoreEntity Create(StoreEntity entity);
    }
}
