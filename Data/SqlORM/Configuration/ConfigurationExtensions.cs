using DriveCentric.Data.SqlORM.Data;
using DriveCentric.Data.SqlORM.Model;
using DriveCentric.Data.SqlORM.Repositories;
using DriveCentric.Model;
using DriveCentric.Utilities.Configuration;
using DriveCentric.Utilities.Data;
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

            services.AddSingleton<IDataAccessor, DataAccessor>();

            services.AddSingleton<IDataRepository<IDriveServer>, GalaxyDataRepository<IDriveServer, DriveServer>>();

            services.AddSingleton<IDriveServerCollection, DriveServerCollection>();
            InstantiateDriveServerCollection(services);

            AddRepositories(services);

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddSingleton<IDataRepository<IDealershipGroup>, GalaxyDataRepository<IDealershipGroup, DealershipGroup>>();

            services.AddSingleton<IDataRepository<ICustomer>, StarDataRepository<ICustomer, Customer>>();
            services.AddSingleton<IDataRepository<IDeal>, StarDataRepository<IDeal, Deal>>();
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
