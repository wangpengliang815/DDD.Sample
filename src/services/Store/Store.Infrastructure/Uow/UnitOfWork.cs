namespace Store.Infrastructure.Uow
{
    using System.Threading.Tasks;

    using DotNetCore.Infra.SeedWork;

    using Store.Infrastructure.Context;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        async Task<bool> IUnitOfWork.Commit()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
