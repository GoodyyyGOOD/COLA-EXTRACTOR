using Sofos2toDatawarehouseSyncer.Application.Interfaces.CacheRepositories;
using Sofos2toDatawarehouseSyncer.Application.Interfaces.Contexts;
using Sofos2toDatawarehouseSyncer.Application.Interfaces.Repositories;
using Sofos2toDatawarehouseSyncer.Infrastructure.CacheRepositories;
using Sofos2toDatawarehouseSyncer.Infrastructure.DbContexts;
using Sofos2toDatawarehouseSyncer.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Sofos2toDatawarehouseSyncer.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            #region Repositories

            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductCacheRepository, ProductCacheRepository>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<IBrandCacheRepository, BrandCacheRepository>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            #endregion Repositories
        }
    }
}