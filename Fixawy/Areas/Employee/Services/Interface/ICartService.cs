using Fixawy.Areas.Employee.DTOS.Response;
using Fixawy.DTOS.Response;

namespace Fixawy.Areas.Employee.Services.Interface
{
    public interface ICartService
    {
        public  Task<CartsResponse> GetCart(string Userid, string? Code, CancellationToken cancellationToken);
        public  Task<AdminResponse> AddInCart(long ProductId, string UserId, decimal Quantity, CancellationToken cancellationToken);
        public  Task<AdminResponse> Increase(long ProductId, string UserId, CancellationToken cancellationToken);
        public  Task<AdminResponse> Decrease(long ProductId, string UserId, CancellationToken cancellationToken);
        public  Task<AdminResponse> Remove(long ProductId, string UserId, CancellationToken cancellationToken);
    }
}
