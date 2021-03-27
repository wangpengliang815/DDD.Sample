namespace DotNetCore.Infra.Abstractions
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>BaseDbContext:全局配置可以在这里添加</summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// <seealso cref="RedPI.Data.IUnitOfWork" />
    public abstract class BaseDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="userProvider">The user provider.</param>
        protected BaseDbContext(
            DbContextOptions options) : base(options)
        {
        }
    }
}
