using DriveCentric.BusinessLogic.Customer;
using DriveCentric.BusinessLogic.Deal;
using DriveCentric.BusinessLogic.DealershipGroup;
using DriveCentric.Data.DataRepository.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DriveCentric.BusinessLogic.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddSingleton<ICustomerBusinessObject, CustomerBusinessObject>();
            services.AddSingleton<IDealBusinessObject, DealBusinessObject>();
            services.AddSingleton<IDealershipGroupBusinessObject, DealershipGroupBusinessObject>();

            services.AddDataRepository();
            return services;
        }
    }
}
