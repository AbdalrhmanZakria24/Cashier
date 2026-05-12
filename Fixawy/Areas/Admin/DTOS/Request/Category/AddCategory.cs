namespace Fixawy.Areas.Admin.DTOS.Request.Category
{
    public class AddCategory
    {
        [Required(ErrorMessage ="enter category name")]
        [MaxLength(1000)]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "enter tenant id")]
        public long TenantId { get; set; }
    }
}
