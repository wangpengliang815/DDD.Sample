namespace Ordering.Infrastructure.Context
{
    using Microsoft.EntityFrameworkCore;

    using Ordering.Domain.Entities;

    public class ApplicationReadOnlyDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "DDDSample";

        public ApplicationReadOnlyDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {

        }

        public DbSet<OrderEntity> Orders { get; private set; }
    }
}
