using System.Threading.Tasks;

namespace DomainCore.SeedWork
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
