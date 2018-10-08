using DriveCentric.BusinessLogic.Configuration;
using DriveCentric.ServiceLayer.Customer;
using DriveCentric.ServiceLayer.Deal;
using DriveCentric.ServiceLayer.DealershipGroup;
using Microsoft.Extensions.DependencyInjection;

namespace DriveCentric.ServiceLayer.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton<IDealService, DealService>();
            services.AddSingleton<IDealershipGroupService, DealershipGroupService>();

            services.AddBusinessLogic();
            return services;
        }
    }
}
