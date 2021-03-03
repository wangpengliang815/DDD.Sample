using System.Threading.Tasks;

namespace DotnetCoreInfra.SeedWork
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
