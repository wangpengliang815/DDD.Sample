namespace Ordering.Infrastructure.Repos
{
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        TEntity Create(TEntity entity);

        TEntity Update(TEntity entity);
    }
}
