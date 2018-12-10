using DriveCentric.BusinessLogic.Implementation;
using DriveCentric.Core.Models;
using DriveCentric.Data.DataRepository.Configuration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DriveCentric.BusinessLogic.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<AbstractValidator<Task>, TaskValidator>();
            services.AddScoped<AbstractValidator<Module>, ModuleValidator>();
            services.AddDataRepository();
            return services;
        }
    }
}