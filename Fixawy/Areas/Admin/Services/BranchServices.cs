using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fixawy.Areas.Admin.Services
{
    public class BranchServices : IBranchServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BranchServices> _logger;

        public BranchServices(IUnitOfWork unitOfWork, ILogger<BranchServices> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<GetResponse<BranchDto>> Get(BranchSerch? branchSerch, Pagination pagination)
        {

            #region Search
            Expression<Func<Branch, bool>>? filter = null;

            if (branchSerch is not null)
            {
                filter = f =>
                (string.IsNullOrEmpty(branchSerch.Name) || f.Name.Contains(branchSerch.Name)) &&
                (string.IsNullOrEmpty(branchSerch.Address) || f.Address.Contains(branchSerch.Address)) &&
                (branchSerch.IsActive == null || f.IsActive == branchSerch.IsActive);
            }

            var allBranch = await _unitOfWork.BranchReposatory.GetQueryable(filter, new Expression<Func<Branch, Object>>[]
            {
                b=>b.Tenant
            });

            #endregion
            #region Pagination
            if (pagination.PageNumber <= 0)
                pagination.PageNumber = 1;

            if (pagination.PageSize <= 0)
                pagination.PageSize = 10;

            var count = await allBranch.CountAsync();

            if (count == 0)
            {
                return new GetResponse<BranchDto>
                {
                    isSuccess = true,
                    message = "No branches found",
                    createAt = DateTime.UtcNow,
                };
            }

            var totalPage = (int)Math.Ceiling((double)count / pagination.PageSize);

            var result = await allBranch
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(c => new BranchDto
                {
                   BranchName = c.Name,
                   Address = c.Address,
                   IsActive = c.IsActive,
                   TenantName = c.Tenant.Name,
                   TenantEmail = c.Tenant.Email,
                })
                .ToListAsync();

            #endregion

            return new GetResponse<BranchDto>
            {
                isSuccess = true,
                message = "All Data ",
                Data = result,
                totalPages = totalPage,
                totalCount = count,
                pageSize = pagination.PageSize,
                currentPage = pagination.PageNumber,
            };
        }

        public async Task<AdminResponse> Create(AddBranch addBranch, CancellationToken cancellationToken)
        {
            var tenant = await _unitOfWork.TenantReposatory.GetOneAsync(c => c.Id == addBranch.TenantId);

            if (tenant is null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Tenant not found",
                    createAt = DateTime.UtcNow,
                };
            }
            ;

            var allBranchsFromUser = await _unitOfWork.BranchReposatory.GetAsync(c => c.TenantId == addBranch.TenantId);

            if (allBranchsFromUser.Count() > 0)
            {
                var isFound = await _unitOfWork.BranchReposatory.GetOneAsync(b => b.Address.ToLower() == addBranch.Address.ToLower() && b.TenantId == addBranch.TenantId);

                if (isFound is not null)
                {
                    return new AdminResponse
                    {
                        isSuccess = false,
                        message = "Branch already exists",
                        createAt = DateTime.UtcNow,
                    };
                }
            }

            var newBranch = new Branch
            {
                TenantId = addBranch.TenantId,
                Name = addBranch.Name,
                Address = addBranch.Address,
                IsActive = addBranch.IsActive,
            };

            try
            {
                await _unitOfWork.BranchReposatory.CreateAsync(newBranch, cancellationToken);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating branch");

                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Exception error",
                    createAt = DateTime.UtcNow,
                };
            }

            return new AdminResponse
            {
                isSuccess = true,
                message = "Branch add successfully",
                createAt = DateTime.UtcNow,
            };

        }

        public async Task<AdminResponse> Update(DTOS.Request.Branch.UpdateBranch? updateBranch, long id)
        {
            if (updateBranch is null)
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Request body is null",
                    createAt = DateTime.UtcNow,
                };

            var tenant = await _unitOfWork.TenantReposatory.GetOneAsync(c => c.Id == updateBranch.TenantId);

            if (tenant is null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Tenant not found",
                    createAt = DateTime.UtcNow,
                };
            }

            var branch = await _unitOfWork.BranchReposatory.GetOneAsync(c => c.TenantId == tenant.Id && c.Id == id);

            if (branch is null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Branch not found for this tenant",
                    createAt = DateTime.UtcNow,
                };
            }

            if (!string.IsNullOrEmpty(updateBranch.Name))
                branch.Name = updateBranch.Name;

            if (!string.IsNullOrEmpty(updateBranch.Address))
                branch.Address = updateBranch.Address;


            if (updateBranch.IsActive is not null)
                updateBranch.IsActive = updateBranch.IsActive.Value;


            _unitOfWork.BranchReposatory.Update(branch);

            await _unitOfWork.CommitAsync();

            return new AdminResponse
            {
                isSuccess = true,
                message = "Branch updated successfully",
                createAt = DateTime.UtcNow,
            };

        }

    }
}
