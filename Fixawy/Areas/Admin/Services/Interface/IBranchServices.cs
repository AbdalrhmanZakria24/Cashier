namespace Fixawy.Areas.Admin.Services.Interface
{
    public interface IBranchServices
    {
        public Task<GetResponse<BranchDto>> Get(BranchSerch? branchSerch, Pagination pagination);
        public  Task<AdminResponse> Create(AddBranch addBranch, CancellationToken cancellationToken);
        public Task<AdminResponse> Update(DTOS.Request.Branch.UpdateBranch? updateBranch, long id);
    }
}
