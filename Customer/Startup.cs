using DriveCentric.BaseService.Interfaces;
using DriveCentric.BaseService.Middleware;
using DriveCentric.BaseService.Services;
using DriveCentric.BusinessLogic.Configuration;
using DriveCentric.Utilities.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PostSharp.Patterns.Caching;
using PostSharp.Patterns.Caching.Backends;
using PostSharp.Patterns.Caching.Backends.Redis;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;

namespace DriveCentric.CustomerService
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

            AddSecurityServices(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHttpContextAccessor();
            services.AddScoped<IContextInfoAccessor, ContextInfoAccessor>();

            services.AddScoped<IBaseService<DriveCentric.Core.Models.Customer>, BaseService<DriveCentric.Core.Models.Customer>>();
            services.AddBusinessLogic();

            services.AddCors(c => c.AddPolicy(Configuration.GetSection("CacheConfig:Name").Value, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "Atlas - Customer Service", Version = "v1" }));
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                ConfigureMemoryCache();
            }
            else
            {
                app.UseHsts();
                ConfigureRedisCache();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Atlas - Customer - V1"));

            app.UseCors(Configuration.GetSection("CacheConfig:Name").Value);

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMiddleware<PermissionsToClaimsMiddleware>();

            app.UseMvc();
        }

        private void ConfigureRedisCache()
        {
            string connectionConfiguration = "redisdev.fzrb0f.ng.0001.use1.cache.amazonaws.com:6379";
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(connectionConfiguration);
            RedisCachingBackendConfiguration configuration = new RedisCachingBackendConfiguration()
            {
                IsLocallyCached = true
            };

            CachingServices.DefaultBackend = RedisCachingBackend.Create(connection, configuration);
        }

        private void ConfigureMemoryCache()
        {
            CachingServices.DefaultBackend = new MemoryCachingBackend();
        }
    }
}
