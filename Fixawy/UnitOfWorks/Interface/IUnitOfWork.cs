namespace Fixawy.UnitOfWorks.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        //public IReposatory<SubscriptionPlan> SubscriptionPlanreposatory { get; }
        public IReposatory<ApplicationUser> ApplicationUserreposatory { get; }
        public IReposatory<Order> Orderreposatory { get; }
        public IReposatory<Payment> Paymentreposatory { get; }
        public IReposatory<Category> Servicereposatory { get; }
        public IReposatory<Review> Reviewreposatory { get; }
        public IReposatory<WorkerSubscription> WorkerSubscriptionreposatory { get; }
        //public IReposatory<Worker> WorkerReposatory { get; }
        public Task CommitAsync();
    }
}
