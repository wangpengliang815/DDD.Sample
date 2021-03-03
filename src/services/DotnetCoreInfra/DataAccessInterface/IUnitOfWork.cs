using System.Threading;
using System.Threading.Tasks;

namespace DotnetCoreInfra.DataAccessInterface
{
    /// <summary>
    /// 一致性提交
    /// </summary>
    public interface IUnitOfWork
    {
        Task<bool> SaveAsync(CancellationToken cancellationToken = default);
    }
}
