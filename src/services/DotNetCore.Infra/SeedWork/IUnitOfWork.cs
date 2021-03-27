using System.Threading.Tasks;

namespace DotNetCore.Infra.SeedWork
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
