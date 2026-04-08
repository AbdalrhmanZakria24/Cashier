namespace Fixawy.UnitOfWorks.Interface
{
    public interface IUnitOfWork : IDisposable
    {
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
        public Task CommitAsync();
    }
}
