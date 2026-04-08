using Fixawy.UnitOfWorks.Interface;

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
            IReposatory<Payment> PaymentReposatory,
            IReposatory<Category> ServiceReposatory,
            IReposatory<Branch> BranchReposatory,
            IReposatory<WorkerSubscription> WorkerSubscriptionReposatory,
            IReposatory<DeviceSync> DeviceSyncReposatory,
            IReposatory<OrderItem> OrderItemReposatory,
            IReposatory<Product> ProductReposatory,
            IReposatory<SyncLogs> SyncLogsReposatory,
            IReposatory<SyncQueue> SyncQueueReposatory,
            IReposatory<Tenant> TenantReposatory,
            ApplicationDBContext  dBContext)
        {
            this.ApplicationUserreposatory = ApplicationUserReposatory;
            this.Subscription = Subscriptions;
            this.SubscriptionsLog = SubscriptionsLogs;
            this.Plan = Plan;
            this.Orderreposatory = OrderReposatory;
            this.Paymentreposatory = PaymentReposatory;
            this.Servicereposatory = ServiceReposatory;
            this.BranchReposatory = BranchReposatory;
            this.WorkerSubscriptionreposatory = WorkerSubscriptionReposatory;
            this.DeviceSyncReposatory = DeviceSyncReposatory;
            this.OrderItemReposatory = OrderItemReposatory;
            this.ProductReposatory = ProductReposatory;
            this.SyncLogsReposatory = SyncLogsReposatory;
            this.SyncQueueReposatory = SyncQueueReposatory;
            this.TenantReposatory = TenantReposatory;
            _dBContext = dBContext;
        }
        public IReposatory<ApplicationUser> ApplicationUserreposatory { get; }
        public IReposatory<Order> Orderreposatory { get; }
        public IReposatory<Payment> Paymentreposatory { get; }
        public IReposatory<Category> Servicereposatory { get; }
        public IReposatory<Branch> BranchReposatory { get; }
        public IReposatory<WorkerSubscription> WorkerSubscriptionreposatory { get; }
        public IReposatory<DeviceSync> DeviceSyncReposatory { get; }
        public IReposatory<OrderItem> OrderItemReposatory { get; }
        public IReposatory<Product> ProductReposatory { get; }
        public IReposatory<SyncLogs> SyncLogsReposatory { get; }
        public IReposatory<SyncQueue> SyncQueueReposatory { get; }
        public IReposatory<Tenant> TenantReposatory { get; }
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
    }
}
