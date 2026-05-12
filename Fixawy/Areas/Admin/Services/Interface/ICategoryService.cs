namespace Fixawy.Areas.Admin.Services.Interface
{
    public interface ICategoryService
    {
        public Task<AdminResponse> Delete(long id);
        public  Task<AdminResponse> Update(UpdateCategory updateCategory, long id);
        public  Task<AdminResponse> Create(AddCategory addCategory, CancellationToken cancellationToken);
        public  Task<GetResponse<CategoryDto>> Get(CategorySerch? categorySerch, Pagination pagination);
    }
}
