namespace Fixawy.Areas.Admin.Model
{
    public class SyncLogs
    {
        public long Id { get; set; }

        public long TenantId { get; set; }
        public Tenant Tenants { get; set; } = null!;

        public long BranchId { get; set; }
        public Branch Branches { get; set; }

        public SyncLogsType syncLogs {  get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
