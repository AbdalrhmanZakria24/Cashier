using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cashier.Application
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssembly(
                Assembly.GetExecutingAssembly());

            return services;
        }
    }
}