using DriveCentric.Data.SqlORM.Repositories;
using DriveCentric.Model;
using DriveCentric.Model.Interfaces;
using DriveCentric.Utilities.Configuration; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace DriveCentric.Data.SqlORM.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddSqlOrmLite(this IServiceCollection services)
        {
            ConfigureConnectionFactory(services);
              
            services.AddSingleton<IDataRepository<DriveServer>, GalaxyDataRepository<DriveServer>>(); 
            services.AddSingleton<IDriveServerCollection, DriveServerCollection>();
            InstantiateDriveServerCollection(services);

            AddRepositories(services);

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<IDataRepository<DealershipGroup>, GalaxyDataRepository<DealershipGroup>>();

            services.AddSingleton<IDataRepository<Customer>, StarDataRepository<Customer>>();
            services.AddSingleton<IDataRepository<Deal>, StarDataRepository<Deal>>();
            services.AddSingleton<IDataRepository<Task>, StarDataRepository<Task>>();
        }

        private static void InstantiateDriveServerCollection(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var driveServerCollection = serviceProvider.GetService<IDriveServerCollection>();
        }

        private static void ConfigureConnectionFactory(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<IConfiguration>();

            var connectionString = configuration.GetSection("SqlDBInfo:ConnectionString").Value;

            services.AddSingleton<IDbConnectionFactory>(
                _ => new OrmLiteConnectionFactory(
                    connectionString,
                    SqlServerDialect.Provider
                    )
                );
        }
    }
}
