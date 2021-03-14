namespace Store.Infrastructure.Repos
{
    using Store.Infrastructure.Context;

    public class OrderRepository : IOrderRepository
    {
        protected readonly ApplicationDbContext DbContext;

        public OrderRepository(ApplicationDbContext context)
        {
            DbContext = context;
        }
    }
}
