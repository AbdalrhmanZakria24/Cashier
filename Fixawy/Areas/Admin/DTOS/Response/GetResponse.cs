namespace Fixawy.Areas.Admin.DTOS.Response
{
    public class GetResponse<T> :AdminResponse
    {
        public ICollection<T> Data {  get; set; } = new List<T>();
        public int? totalTenant { get; set; }
        public int? totalPages { get; set; }
        public int? currentPage { get; set; }
        public int? pageSize { get; set; }
    }
}
