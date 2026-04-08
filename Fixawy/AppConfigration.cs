using System.Runtime.CompilerServices;

namespace Fixawy
{
    public static class AppConfigration
    {
        public static void RegisterConfig(this IServiceCollection services )
        {
            // Repository
            //services.AddScoped<IReposatory<SubscriptionPlan>, Repositories.Reposatory<SubscriptionPlan>>();
            services.AddScoped<IReposatory<ApplicationUser>, Repositories.Reposatory<ApplicationUser>>();
            services.AddScoped<IReposatory<Order>, Repositories.Reposatory<Order>>();
            //services.AddScoped<IReposatory<Worker>, Repositories.Reposatory<Worker>>();
            services.AddScoped<IReposatory<Payment>, Repositories.Reposatory<Payment>>();
            services.AddScoped<IReposatory<Category>, Repositories.Reposatory<Category>>();
            services.AddScoped<IReposatory<Review>, Repositories.Reposatory<Review>>();
            services.AddScoped<IReposatory<WorkerSubscription>, Repositories.Reposatory<WorkerSubscription>>();
            services.AddScoped<IReposatory<SubscriptionLogs>, Repositories.Reposatory<SubscriptionLogs>>();
            services.AddScoped<IReposatory<Subscription>, Repositories.Reposatory<Subscription>>();
            services.AddScoped<IReposatory<Plan>, Repositories.Reposatory<Plan>>();

            //Unit of work
            services.AddScoped<IUnitOfWork,UnitOfWork>();

            //Serviecs
            services.AddScoped<IAccountService, AccountServic>();
        }
    }
}
