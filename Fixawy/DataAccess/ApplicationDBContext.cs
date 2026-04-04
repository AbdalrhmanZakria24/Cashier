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
        public DbSet<SubscriptionPlan> subscriptionPlans { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Service> services { get; set; }
    }
}
