namespace Fixawy.Areas.Admin.Services.Interface
{
    public interface IProductService
    {
        public Task<GetResponse<Product>> Get(ProductSearch? product, Pagination pagination);
        public Task<AdminResponse> Create(AddProduct addProduct, CancellationToken cancellationToken);
        public Task<AdminResponse> Update(UpdateProduct updateProduct, long id, CancellationToken cancellationToken);
        public Task<AdminResponse> Delete(long id, long branchId);
    }
}
