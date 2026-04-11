using Fixawy.EmailSender.Interface;
using System.Runtime.CompilerServices;

namespace Fixawy
{
    public static class AppConfigration
    {
        public static void RegisterConfig(this IServiceCollection services )
        {
            // Repository
            services.AddScoped<IReposatory<ApplicationUser>, Repositories.Reposatory<ApplicationUser>>();
            services.AddScoped<IReposatory<ApplicationuserOtp>, Repositories.Reposatory<ApplicationuserOtp>>();
            services.AddScoped<IReposatory<Branch>, Repositories.Reposatory<Branch>>();
            services.AddScoped<IReposatory<Payment>, Repositories.Reposatory<Payment>>();
            services.AddScoped<IReposatory<Category>, Repositories.Reposatory<Category>>();
            services.AddScoped<IReposatory<Order>, Repositories.Reposatory<Order>>();
            services.AddScoped<IReposatory<OrderItem>, Repositories.Reposatory<OrderItem>>();
            services.AddScoped<IReposatory<Product>, Repositories.Reposatory<Product>>();
            services.AddScoped<IReposatory<SyncLogs>, Repositories.Reposatory<SyncLogs>>();
            services.AddScoped<IReposatory<DeviceSync>, Repositories.Reposatory<DeviceSync>>();
            services.AddScoped<IReposatory<SyncQueue>, Repositories.Reposatory<SyncQueue>>();
            services.AddScoped<IReposatory<Tenant>, Repositories.Reposatory<Tenant>>();
            services.AddScoped<IReposatory<WorkerSubscription>, Repositories.Reposatory<WorkerSubscription>>();
            services.AddScoped<IReposatory<SubscriptionLogs>, Repositories.Reposatory<SubscriptionLogs>>();
            services.AddScoped<IReposatory<Subscription>, Repositories.Reposatory<Subscription>>();
            services.AddScoped<IReposatory<Plan>, Repositories.Reposatory<Plan>>();

            //Unit of work
            services.AddScoped<IUnitOfWork,UnitOfWork>();

            //Serviecs
            services.AddScoped<IAccountService, AccountService>();
            services.AddTransient<ITokenService, TokenService>();

            //Db Initializar
            services.AddScoped<IDBInitializar, DBInitializar>();

            //Email Sender
            services.AddScoped<IEmailSeder, EmailSender.EmailSender>(); 

        }
    }
}
