using Fixawy.Areas.Employee.Model;
using Fixawy.UnitOfWorks.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fixawy.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _dBContext;

        public UnitOfWork(
            IReposatory<SubscriptionLogs> SubscriptionsLogs,
            IReposatory<Subscription> Subscriptions,
            IReposatory<Plan> Plan,
            IReposatory<ApplicationUser> ApplicationUserReposatory,
            IReposatory<Order> OrderReposatory,
           // IReposatory<Payment> PaymentReposatory,
            IReposatory<Category> ServiceReposatory,
            IReposatory<Branch> BranchReposatory,
            IReposatory<BranchProduct> BranchProductReposatory,
            IReposatory<DeviceSync> DeviceSyncReposatory,
            IReposatory<OrderItem> OrderItemReposatory,
            IReposatory<Product> ProductReposatory,
            IReposatory<SyncLogs> SyncLogsReposatory,
            IReposatory<ApplicationuserOtp> ApplicationuserOtpReposatory,
            IReposatory<SyncQueue> SyncQueueReposatory,
            IReposatory<Tenant> TenantReposatory,
            IReposatory<Cart> Cartreposatory,
            IReposatory<CartItem> CartItemreposatory,
            IReposatory<Promotion> Promotionreposatory,
            ApplicationDBContext  dBContext)
        {
            this.ApplicationUserreposatory = ApplicationUserReposatory;
            this.Subscription = Subscriptions;
            this.SubscriptionsLog = SubscriptionsLogs;
            this.Plan = Plan;
            this.Orderreposatory = OrderReposatory;
            //this.Paymentreposatory = PaymentReposatory;
            this.Categoryreposatory = ServiceReposatory;
            this.BranchReposatory = BranchReposatory;
            this.BranchProductReposatory = BranchProductReposatory;
            this.DeviceSyncReposatory = DeviceSyncReposatory;
            this.OrderItemReposatory = OrderItemReposatory;
            this.ProductReposatory = ProductReposatory;
            this.SyncLogsReposatory = SyncLogsReposatory;
            this.ApplicationuserOtpReposatory = ApplicationuserOtpReposatory;
            this.SyncQueueReposatory = SyncQueueReposatory;
            this.TenantReposatory = TenantReposatory;
            this.Cartreposatory = Cartreposatory;
            this.CartItemreposatory = CartItemreposatory;
            this.Promotionreposatory = Promotionreposatory;
            _dBContext = dBContext;
        }
        public IReposatory<ApplicationUser> ApplicationUserreposatory { get; }
        public IReposatory<Order> Orderreposatory { get; }
       // public IReposatory<Payment> Paymentreposatory { get; }
        public IReposatory<Category> Categoryreposatory { get; }
        public IReposatory<Branch> BranchReposatory { get; }
        public IReposatory<BranchProduct> BranchProductReposatory { get; }
        public IReposatory<DeviceSync> DeviceSyncReposatory { get; }
        public IReposatory<OrderItem> OrderItemReposatory { get; }
        public IReposatory<Product> ProductReposatory { get; }
        public IReposatory<SyncLogs> SyncLogsReposatory { get; }
        public IReposatory<ApplicationuserOtp> ApplicationuserOtpReposatory { get; }
        public IReposatory<SyncQueue> SyncQueueReposatory { get; }
        public IReposatory<Tenant> TenantReposatory { get; }
        public IReposatory<Cart> Cartreposatory { get; }
        public IReposatory<CartItem> CartItemreposatory { get; }
        public IReposatory<Promotion> Promotionreposatory { get; }
        public IReposatory<Subscription> Subscription { get; }
        public IReposatory<SubscriptionLogs> SubscriptionsLog { get; }
        public IReposatory<Plan> Plan { get; }

        public void Dispose()
        {
            _dBContext.Dispose();   
        }

        public async Task CommitAsync()
        {
            await _dBContext.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dBContext.Database.BeginTransactionAsync();
        }
    }
}
