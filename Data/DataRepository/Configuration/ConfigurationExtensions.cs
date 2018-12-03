using DriveCentric.Core.Interfaces;
using DriveCentric.Data.DataRepository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;

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

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository, Repository>();
            InstantiateDriveServerCollection(services);

            return services;
        }

        private static void InstantiateDriveServerCollection(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
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