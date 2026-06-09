using Fixawy.Areas.Employee.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fixawy.UnitOfWorks.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        public IReposatory<ApplicationUser> ApplicationUserreposatory { get; }
        public IReposatory<Order> Orderreposatory { get; }
        public IReposatory<Payment> Paymentreposatory { get; }
        public IReposatory<PaymentTransaction> PaymentTransactionReposatory { get; }
        public IReposatory<Category> Categoryreposatory { get; }
        public IReposatory<Invoice> InvoiceReposatory { get; }
        public IReposatory<Branch> BranchReposatory { get; }
        public IReposatory<DeviceSync> DeviceSyncReposatory { get; }
        public IReposatory<OrderItem> OrderItemReposatory { get; }
        public IReposatory<Product> ProductReposatory { get; }
        public ICartItemReposatory RemoveRangeCartItemReposatories { get; }
        public IReposatory<SyncLogs> SyncLogsReposatory { get; }
        public IReposatory<SyncQueue> SyncQueueReposatory { get; }
        public IReposatory<Cart> Cartreposatory { get; }
        public IReposatory<CartItem> CartItemreposatory { get; }
        public IReposatory<Promotion> Promotionreposatory { get; }
        public IReposatory<Tenant> TenantReposatory { get; }
        public IReposatory<Subscription> Subscription { get; }
        public IReposatory<SubscriptionLogs> SubscriptionsLog { get; }
        public IReposatory<Plan> Plan { get; }
        public IReposatory<ApplicationuserOtp> ApplicationuserOtpReposatory { get; }
        public IReposatory<BranchProduct> BranchProductReposatory { get; }
        public Task CommitAsync();
        public Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
