using System;
using System.Linq;
using System.Security.Claims;
using DriveCentric.BaseService.Interfaces;
using DriveCentric.BaseService.Middleware;
using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Configuration;
using DriveCentric.BusinessLogic.Implementation;
using DriveCentric.Core.Interfaces;
using DriveCentric.Core.Models;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ServiceStack.Text;
using Swashbuckle.AspNetCore.Swagger;

namespace DriveCentric.TaskService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            services.AddCors(c => c.AddPolicy("DrivePolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            AddSecurityServices(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHttpContextAccessor();
            services.AddScoped<IContextInfoAccessor, ContextInfoAccessor>();

            services.AddScoped<IBaseService<UserTask>, BaseService<UserTask>>();
            services.AddBusinessLogic();

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "Atlas - Task Service", Version = "v1" }));
        }

        private void AddSecurityServices(IServiceCollection services)
        {
            services.Configure<JwtBearerOptions>(Configuration.GetSection("Authentication:Cognito"));
            var serviceProvider = services.BuildServiceProvider();
            var authOptions = serviceProvider.GetService<IOptions<JwtBearerOptions>>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.MetadataAddress = authOptions.Value.MetadataAddress;
                options.SaveToken = authOptions.Value.SaveToken;
                options.IncludeErrorDetails = authOptions.Value.IncludeErrorDetails;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = authOptions.Value.TokenValidationParameters.ValidateAudience,
                    ValidateIssuer = authOptions.Value.TokenValidationParameters.ValidateIssuer
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IContextInfoAccessor contextInfoAccessor,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            JsConfig.TreatEnumAsInteger = true;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Atlas - Task - V1"));

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMiddleware<PermissionsToClaimsMiddleware>();

            app.UseMvc();
            app.UseCors("DrivePolicy");
        }

        private static async System.Threading.Tasks.Task AddStarClaims(
            IContextInfoAccessor contextInfoAccessor,
            IUnitOfWork unitOfWork,
            ClaimsIdentity user,
            Guid userGuid)
        {
            var starService = new BaseService<StarClaimPermission>(contextInfoAccessor, unitOfWork, new StarClaimPermissionValidator());
            var starPermissions =
                await starService.GetAllByExpressionAsync(
                    x => x.ExternalId == userGuid, new PageableSearch());

            foreach (var permission in starPermissions.Data)
            {
                user.AddClaim(new Claim(permission.Key, permission.Value));
            }
        }

        private static async System.Threading.Tasks.Task AddGalaxyClaims(
            IContextInfoAccessor contextInfoAccessor,
            IUnitOfWork unitOfWork,
            ClaimsIdentity user,
            Guid userGuid)
        {
            var service = new BaseService<GalaxyClaimPermission>(contextInfoAccessor, unitOfWork, new GalaxyClaimPermissionValidator());
            var permissions =
                await service.GetAllByExpressionAsync(
                    x => x.ExternalId == userGuid, new PageableSearch());

            foreach (var permission in permissions.Data)
            {
                user.AddClaim(new Claim(permission.Key, permission.Value));
            }
        }
    }
}