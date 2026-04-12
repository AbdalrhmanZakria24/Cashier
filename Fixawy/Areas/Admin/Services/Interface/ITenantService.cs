namespace Fixawy.Areas.Admin.Services.Interface
{
    public interface ITenantService
    {
        public Task<GetResponse<Tenant>> GetAll(TenantSearch? tenantSearch, Pagination pagination);
        public  Task<AdminResponse> Create(AddTenant addTenant, CancellationToken cancellationToken);
        public  Task<AdminResponse> Update(UpdateTenant updateTenant, int tenantId);
        public  Task<AdminResponse> Delete(int tenantId);
    }
}
