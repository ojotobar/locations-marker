using Hangfire;
using Hangfire.Console;
using Hangfire.InMemory;
using LocationMarker.Data.Implementations;
using LocationMarker.Data.Interfaces;
using LocationMarker.Service.Implementations;
using LocationMarker.Service.Interfaces;
using LocationsMarker.API.Filters;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;


namespace LocationsMarker.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureHttpClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration["CSAPI:BaseAddress"]!),
                Timeout = TimeSpan.FromMinutes(5)
            };

            httpClient.DefaultRequestHeaders.Add("X-CSCAPI-KEY", configuration["CSAPI:APIKEY"]);
            services.AddSingleton(httpClient);
        }

        public static void ConfigureController(this IServiceCollection services) =>
            services
            .AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddNewtonsoftJson(x =>
                x.SerializerSettings.ContractResolver = new DefaultContractResolver());

        public static void ConfigureSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Location Marker Api",
                    Version = "v1.0",
                    Description = "Location Marker API by Ojo Toba Rufus",
                    Contact = new OpenApiContact
                    {
                        Name = "Toba R. Ojo",
                        Email = "ojotobar@gmail.com",
                        Url = new Uri("https://ojotobar.netlify.app")
                    }
                });
            });

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<HangfireLogAttribute>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        public static void ConfigureHangfireClient(this IServiceCollection services)
        {
            services.AddHangfire((provider, config) =>
            {
                config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseInMemoryStorage(new InMemoryStorageOptions
                {
                    MaxExpirationTime = TimeSpan.FromHours(6),
                    StringComparer = StringComparer.Ordinal,
                    IdType = InMemoryStorageIdType.Guid
                })
                .WithJobExpirationTimeout(TimeSpan.FromHours(5))
                .UseConsole()
                //.UseRecurringJob(typeof(IRecurringJobService))
                .UseFilter(provider.GetRequiredService<HangfireLogAttribute>())
                .UseFilter(new AutomaticRetryAttribute()
                {
                    Attempts = 5,
                    OnAttemptsExceeded = AttemptsExceededAction.Delete
                });
            });
        }

        public static void ConfigureHangfireServer(this IServiceCollection services)
        {
            services.AddHangfireServer(options =>
            {
                options.Queues = ["recurring", "default"];
                options.SchedulePollingInterval = TimeSpan.FromMinutes(1);
            });
        }
    }
}
