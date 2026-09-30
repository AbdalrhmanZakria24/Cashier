using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cashier.Application.Interface
{
    public interface IUnitOfWork
    {
        public IRepository<ApplicationUser> ApplicationUserreposatory { get; }
        public IRepository<Order> Orderreposatory { get; }
        public IRepository<Payment> Paymentreposatory { get; }
        public IRepository<PaymentTransaction> PaymentTransactionReposatory { get; }
        public IRepository<Category> Categoryreposatory { get; }
        public IRepository<Branch> BranchReposatory { get; }
        public IRepository<BranchProduct> BranchProductReposatory { get; }
        public IRepository<DeviceSync> DeviceSyncReposatory { get; }
        public IRepository<OrderItem> OrderItemReposatory { get; }
        public IRepository<Product> ProductReposatory { get; }
        public IRepository<SyncLogs> SyncLogsReposatory { get; }
        public IRepository<ApplicationuserOtp> ApplicationuserOtpReposatory { get; }
        public IRepository<SyncQueue> SyncQueueReposatory { get; }
        public IRepository<Invoice> InvoiceReposatory { get; }
        public IRepository<Cart> Cartreposatory { get; }
        public IRepository<CartItem> CartItemreposatory { get; }
        public IRepository<Promotion> Promotionreposatory { get; }
        public IRepository<RefreshToken> RefreshTokenreposatory { get; }
        public ICartItemReposatory RemoveRangeCartItemReposatories { get; }
        public void Dispose();

        public  Task CommitAsync(CancellationToken cancellationToken);

        public  Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
