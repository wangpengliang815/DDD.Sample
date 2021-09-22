namespace DotNetCore.Infra.SeedWork
{
    using System.Threading.Tasks;

    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
