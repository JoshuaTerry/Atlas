using System;
using DriveCentric.Data.SqlORM.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
