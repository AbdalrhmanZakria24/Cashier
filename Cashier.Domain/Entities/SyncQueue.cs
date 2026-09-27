using Cashier.Domain.Enums;

namespace Cashier.Domain.Entities
{
    public class SyncQueue
    {
        public long Id { get; set; }

        public long TenantId { get; set; }
        public long BranchId { get; set; }

        public string Entity { get; set; } = null!;
        public long EntityId { get; set; }

        public SyncAction Action { get; set; }

        public string Payload { get; set; } = null!;

        public int Version { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }

        public SyncStatus Status { get; set; } = SyncStatus.Pending;

        public int RetryCount { get; set; } = 0;

        public long? DeviceId { get; set; }
    }
}
