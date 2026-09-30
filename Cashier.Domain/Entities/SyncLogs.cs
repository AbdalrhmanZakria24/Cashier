using Cashier.Domain.Enums;

namespace Cashier.Domain.Entities
{
    public class SyncLogs
    {
        public long Id { get; set; }

        public long BranchId { get; set; }
        public Branch Branches { get; set; }

        public SyncLogsType syncLogs {  get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
