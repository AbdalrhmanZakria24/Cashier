using Fixawy.UnitOfWorks.Interface;

namespace Fixawy.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _dBContext;

        public UnitOfWork(IReposatory<SubscriptionPlan> SubscriptionPlanReposatory,
            IReposatory<ApplicationUser> ApplicationUserReposatory,
            IReposatory<Order> OrderReposatory,
            IReposatory<Payment> PaymentReposatory,
            IReposatory<Service> ServiceReposatory,
            IReposatory<Review> ReviewReposatory,
            IReposatory<WorkerSubscription> WorkerSubscriptionReposatory,
            IReposatory<Worker> WorkerReposatory,
            ApplicationDBContext  dBContext)
        {
            this.SubscriptionPlanreposatory = SubscriptionPlanReposatory;
            this.ApplicationUserreposatory = ApplicationUserReposatory;
            this.Orderreposatory = OrderReposatory;
            this.Paymentreposatory = PaymentReposatory;
            this.Servicereposatory = ServiceReposatory;
            this.Reviewreposatory = ReviewReposatory;
            this.WorkerSubscriptionreposatory = WorkerSubscriptionReposatory;
            this.WorkerReposatory = WorkerReposatory;
            _dBContext = dBContext;
        }

        public IReposatory<SubscriptionPlan> SubscriptionPlanreposatory { get; }
        public IReposatory<ApplicationUser> ApplicationUserreposatory { get; }
        public IReposatory<Order> Orderreposatory { get; }
        public IReposatory<Payment> Paymentreposatory { get; }
        public IReposatory<Service> Servicereposatory { get; }
        public IReposatory<Review> Reviewreposatory { get; }
        public IReposatory<WorkerSubscription> WorkerSubscriptionreposatory { get; }
        public IReposatory<Worker> WorkerReposatory { get; }

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
