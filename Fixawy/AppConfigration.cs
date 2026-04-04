using System.Runtime.CompilerServices;

namespace Fixawy
{
    public static class AppConfigration
    {
        public static void RegisterConfig(this IServiceCollection services )
        {
            // Repository
            services.AddScoped<IReposatory<SubscriptionPlan>, Repositories.Reposatory<SubscriptionPlan>>();
            services.AddScoped<IReposatory<ApplicationUser>, Repositories.Reposatory<ApplicationUser>>();
            services.AddScoped<IReposatory<Order>, Repositories.Reposatory<Order>>();
            services.AddScoped<IReposatory<Payment>, Repositories.Reposatory<Payment>>();
            services.AddScoped<IReposatory<Service>, Repositories.Reposatory<Service>>();
            services.AddScoped<IReposatory<Review>, Repositories.Reposatory<Review>>();
            services.AddScoped<IReposatory<WorkerSubscription>, Repositories.Reposatory<WorkerSubscription>>();
        }
    }
}
