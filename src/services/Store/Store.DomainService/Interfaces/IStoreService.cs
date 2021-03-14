using System.Threading.Tasks;

using Store.Domain.AggregateModels;

namespace Store.DomainService.Interfaces
{
    public interface IStoreService
    {
        Task<StoreModel> CreateAsync(
            StoreModel model);
    }
}
