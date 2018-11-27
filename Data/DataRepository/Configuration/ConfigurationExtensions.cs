using System;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Data.SqlORM.Configuration;
using DriveCentric.Data.SqlORM.Repositories;
using DriveCentric.Utilities.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace DriveCentric.Data.DataRepository.Configuration
{
    public static class ConfigurationExtensions
    {
        private const string SQLORMLITE = "SqlOrmLite";

        public static IServiceCollection AddDataRepository(this IServiceCollection services)
        {
            ConfigureORM(services);

            return services;
        }

        private static void ConfigureORM(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<IConfiguration>();

            var orm = configuration.GetSection("DataRepository:ORM").Value;

            if (orm.Equals(SQLORMLITE, StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddSqlOrmLite();
            }
        }

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
            services.AddSingleton<IDataRepository<Module>, GalaxyDataRepository<Module>>();

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
