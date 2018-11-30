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
            services.AddScoped<ICustomerLogic, CustomerLogic>();
            services.AddScoped<IDealLogic, DealLogic>();
            services.AddScoped<IDealershipGroupLogic, DealershipGroupLogic>();
            services.AddScoped<ITaskLogic, TaskLogic>();
            services.AddScoped<IModuleLogic, ModuleLogic>();

            services.AddDataRepository();
            return services;
        }
    }
}
