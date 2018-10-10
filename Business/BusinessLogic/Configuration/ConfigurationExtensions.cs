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
            services.AddSingleton<ICustomerLogic, CustomerLogic>();
            services.AddSingleton<IDealLogic, DealLogic>();
            services.AddSingleton<IDealershipGroupLogic, DealershipGroupLogic>();

            services.AddDataRepository();
            return services;
        }
    }
}
