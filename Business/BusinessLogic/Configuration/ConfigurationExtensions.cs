using DriveCentric.BusinessLogic.Implementation;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Data.DataRepository;
using DriveCentric.Data.DataRepository.Configuration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DriveCentric.BusinessLogic.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<LogicBase<UserTask>, TaskLogic>();
            services.AddScoped<AbstractValidator<Module>, ModuleValidator>();

            services.AddScoped<IReadOnlyUnitOfWork, UnitOfWork>();
            services.AddDataRepository();
            return services;
        }
    }
}