using Sofos2toDatawarehouseSyncer.Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sofos2toDatawarehouseSyncer.Application.Interfaces.CacheRepositories
{
    public interface IProductCacheRepository
    {
        Task<List<Product>> GetCachedListAsync();

        Task<Product> GetByIdAsync(int brandId);
    }
}