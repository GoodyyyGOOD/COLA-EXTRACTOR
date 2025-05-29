using Sofos2toDatawarehouseSyncer.Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sofos2toDatawarehouseSyncer.Application.Interfaces.CacheRepositories
{
    public interface IBrandCacheRepository
    {
        Task<List<Brand>> GetCachedListAsync();

        Task<Brand> GetByIdAsync(int brandId);
    }
}