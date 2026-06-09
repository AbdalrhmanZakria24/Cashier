namespace Fixawy.Areas.Employee.Services.Interface
{
    public interface IPaymentService
    {
        public Task<AdminResponse> CreateStripePaymentIntent(string userId, CancellationToken cancellationToken);
    }
}
