namespace Fixawy.Areas.Identity.Services.Interface
{
    public interface IAccountService
    {
        public Task<IdentityResponse> RegisterWorker(RegisterWorker registerWorker, CancellationToken cancellationToken);
    }
}
