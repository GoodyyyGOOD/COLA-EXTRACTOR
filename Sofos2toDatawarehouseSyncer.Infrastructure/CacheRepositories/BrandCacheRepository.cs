﻿using Sofos2toDatawarehouseSyncer.Application.Interfaces.CacheRepositories;
using Sofos2toDatawarehouseSyncer.Application.Interfaces.Repositories;
using Sofos2toDatawarehouseSyncer.Domain.Entities.Catalog;
using Sofos2toDatawarehouseSyncer.Infrastructure.CacheKeys;
using AspNetCoreHero.Extensions.Caching;
using AspNetCoreHero.ThrowR;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sofos2toDatawarehouseSyncer.Infrastructure.CacheRepositories
{
    public class BrandCacheRepository : IBrandCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IBrandRepository _brandRepository;

        public BrandCacheRepository(IDistributedCache distributedCache, IBrandRepository brandRepository)
        {
            _distributedCache = distributedCache;
            _brandRepository = brandRepository;
        }

        public async Task<Brand> GetByIdAsync(int brandId)
        {
            string cacheKey = BrandCacheKeys.GetKey(brandId);
            var brand = await _distributedCache.GetAsync<Brand>(cacheKey);
            if (brand == null)
            {
                brand = await _brandRepository.GetByIdAsync(brandId);
                Throw.Exception.IfNull(brand, "Brand", "No Brand Found");
                await _distributedCache.SetAsync(cacheKey, brand);
            }
            return brand;
        }

        public async Task<List<Brand>> GetCachedListAsync()
        {
            string cacheKey = BrandCacheKeys.ListKey;
            var brandList = await _distributedCache.GetAsync<List<Brand>>(cacheKey);
            if (brandList == null)
            {
                brandList = await _brandRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, brandList);
            }
            return brandList;
        }
    }
}