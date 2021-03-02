namespace Ordering.Infrastructure.Context
{
    using Microsoft.EntityFrameworkCore;

    using Ordering.Domain.Entities;

    public class ApplicationDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "DDDSample";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {

        }

        public DbSet<OrderEntity> Orders { get; set; }
    }
}
