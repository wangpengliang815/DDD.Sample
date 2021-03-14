namespace Store.Infrastructure.Context
{
    using Microsoft.EntityFrameworkCore;

    using Store.Infrastructure.Entities;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions options) : base(options)
        {
        }

        public DbSet<StoreEntity> Stores { get; set; }
    }
}
