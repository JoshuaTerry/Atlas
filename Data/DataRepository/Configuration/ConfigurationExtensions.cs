using System;
using DriveCentric.Data.SqlORM.Configuration;
//using DriveCentric.Data.SqlORM.Repositories;
using DriveCentric.Model;
using DriveCentric.Model.Interfaces;
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

            services.AddScoped<IDriveServerCollection, DriveServerCollection>();
            InstantiateDriveServerCollection(services);

            return services;
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
