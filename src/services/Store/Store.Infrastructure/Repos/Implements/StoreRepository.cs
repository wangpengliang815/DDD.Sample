namespace Store.Infrastructure.Repos
{
    using Store.Infrastructure.Context;
    using Store.Infrastructure.Entities;

    public class StoreRepository : IStoreRepository
    {
        protected readonly ApplicationDbContext DbContext;

        public StoreRepository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public StoreEntity Create(StoreEntity entity)
        {
            DbContext.Stores.Add(entity);
            return entity;
        }
    }
}
