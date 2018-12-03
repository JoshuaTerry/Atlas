using DriveCentric.BusinessLogic.Implementation;
using DriveCentric.BusinessLogic.Interfaces;
using DriveCentric.Data.DataRepository.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DriveCentric.BusinessLogic.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        { 
            services.AddDataRepository();
            return services;
        }
    }
}
