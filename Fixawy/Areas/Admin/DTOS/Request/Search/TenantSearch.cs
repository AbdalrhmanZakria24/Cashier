namespace Fixawy.Areas.Admin.DTOS.Request.Search
{
    public class TenantSearch
    {
        public string? name { get; set; }
        public string? email { get; set; }
        public bool? haveBranchs { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
