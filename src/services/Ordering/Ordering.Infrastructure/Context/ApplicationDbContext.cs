namespace Ordering.Infrastructure.Context
{
    using Microsoft.EntityFrameworkCore;

    using Ordering.Domain.AggregateModels;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
