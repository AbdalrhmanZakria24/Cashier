namespace Fixawy.Areas.Admin.Model
{
    public class Worker
    {
        public int Id { get; set; }

        public string userId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int serviceId { get; set; }
        public Service Service { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string Location { get; set; } = string.Empty;

        public string NationalIdImage { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
        public string? Description { get; set; }

        public bool Isvalid { get; set; }
    }
}
