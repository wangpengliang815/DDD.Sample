namespace Ordering.Infrastructure.Context
{
    using Microsoft.EntityFrameworkCore;

    using Ordering.Infrastructure.Entities;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions options) : base(options)
        {

        }

        public DbSet<OrderEntity> Orders { get; set; }

        public DbSet<OrderDetailEntity> OrderDetails { get; set; }
    }
}
