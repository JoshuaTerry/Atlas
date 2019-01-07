using DriveCentric.Core.Interfaces;
using DriveCentric.Data.DataRepository.Interfaces;
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
                services.AddSqlOrmLite(provider);
            }
        }

        public static IServiceCollection AddSqlOrmLite(this IServiceCollection services, ServiceProvider provider)
        {
            ConfigureConnectionFactory(services, provider);

            services.AddScoped<IDriveServerCollection, DriveServerCollection>();
            services.AddScoped<IDatabaseCollectionManager, DatabaseCollectionManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static void ConfigureConnectionFactory(IServiceCollection services, ServiceProvider provider)
        {
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