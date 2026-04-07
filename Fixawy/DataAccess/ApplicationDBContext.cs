using Fixawy.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fixawy.DataAccess
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) :base(options)
        {
        }

        public DbSet<Payment> payments { get; set; }
        public DbSet<WorkerSubscription> workerSubscriptions { get; set; }
        //public DbSet<SubscriptionPlan> subscriptionPlans { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Category> categories { get; set; }
        //public DbSet<Worker> workers { get; set; }
        public DbSet<Subscription> subscriptions { get; set; }
        public DbSet<SubscriptionLogs> subscriptionLogs { get; set; }
        public DbSet<Plan> plan { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // مهم جداً

            // Existing indexes
            builder.Entity<Subscription>()
                .HasIndex(x => new { x.TenantId, x.UserId });

            builder.Entity<SubscriptionLogs>()
                .HasIndex(x => new { x.SubscriptionId, x.CreatedAt });

            builder.Entity<Subscription>()
                .HasIndex(x => new { x.TenantId, x.Status });

            // Fix IdentityPasskeyData
            builder.Entity<IdentityPasskeyData>().HasNoKey();
        }
    }
    
}
