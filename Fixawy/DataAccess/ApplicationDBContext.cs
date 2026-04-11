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

        public DbSet<Payment> Payments { get; set; }
        public DbSet<WorkerSubscription> WorkerSubscriptions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionLogs> SubscriptionLogs { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<SyncLogs> SyncLogs { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<DeviceSync> DeviceSyncs { get; set; }
        public DbSet<SyncQueue> SyncQueues { get; set; }
        public DbSet<ApplicationuserOtp> applicationuserOtps { get; set; }

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

            builder.Entity<Product>()
                .HasIndex(p => p.TenantId);

            builder.Entity<Product>()
                .HasIndex(x => new { x.TenantId, x.CategoryId });

            builder.Entity<Category>()
               .HasIndex(p => p.TenantId);

            builder.Entity<Branch>()
               .HasIndex(p => p.TenantId);

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<Order>()
                .HasIndex(p => new { p.TenantId , p.BranchId});

            builder.Entity<Order>()
                .HasIndex(x => new { x.TenantId, x.CreatedAt });

            builder.Entity<SyncQueue>()
                .HasIndex(x => new { x.TenantId, x.Status, x.CreatedAt });

            builder.Entity<SyncQueue>()
                .HasIndex(x => new { x.Entity, x.EntityId });

            // Fix IdentityPasskeyData
            builder.Entity<IdentityPasskeyData>().HasNoKey();

            builder.Entity<Order>()
                .HasOne(x => x.Branch)
                .WithMany()
                .HasForeignKey(x => x.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderItem>()
               .HasOne(x => x.Order)
               .WithMany()
               .HasForeignKey(x => x.OrderId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
               .HasOne(x => x.Category)
               .WithMany()
               .HasForeignKey(x => x.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
               .HasOne(x => x.Tenant)
               .WithMany()
               .HasForeignKey(x => x.TenantId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Branch>()
              .HasOne(b => b.Tenant)
              .WithMany()
              .HasForeignKey(b => b.TenantId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DeviceSync>()
              .HasOne(d => d.Tenant)
              .WithMany()
              .HasForeignKey(d => d.TenantId)
              .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<DeviceSync>()
                .HasOne(d => d.Branch)
                .WithMany()
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SubscriptionLogs>()
                .HasOne(sl => sl.Subscription)
                .WithMany()
                .HasForeignKey(sl => sl.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(x=>x.Tenant)
                .WithMany()
                .HasForeignKey(x=>x.TenantId)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Entity<Product>()
                .Property(p => p.Cost)
                .HasPrecision(18, 2);

            builder.Entity<Order>()
                .Property(o => o.Amount)
                .HasPrecision(18, 2);

            builder.Entity<OrderItem>()
                .Property(i => i.Price)
                .HasPrecision(18, 2);

            builder.Entity<OrderItem>()
               .Property(i => i.Total)
               .HasPrecision(18, 2);

            builder.Entity<Subscription>()
               .Property(s => s.Price)
               .HasPrecision(18, 2);

        }
    }
    
}
