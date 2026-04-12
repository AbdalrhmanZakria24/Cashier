using Fixawy.Areas.Admin.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fixawy.Areas.Admin.Services
{
    public class TenantService : ITenantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TenantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetResponse<Tenant>> GetAll(TenantSearch? tenantSearch, Pagination pagination)
        {

            #region Search

            Expression<Func<Tenant, bool>>? filter = null;

            if (tenantSearch is not null)
            {
                filter = s =>
                (string.IsNullOrEmpty(tenantSearch.name) || s.Name.Contains(tenantSearch.name)) &&
                (string.IsNullOrEmpty(tenantSearch.email) || s.Email.Contains(tenantSearch.email)) &&
                (tenantSearch.IsActive == null || s.IsActive == tenantSearch.IsActive) &&
                (tenantSearch.haveBranchs == null || s.HaveBranches == tenantSearch.haveBranchs) &&
                (tenantSearch.SubscriptionEndDate == null || s.SubscriptionEndDate.Date == tenantSearch.SubscriptionEndDate.Value.Date) &&
                (tenantSearch.CreatedAt == null || s.CreatedAt.Date == tenantSearch.CreatedAt.Value.Date);
            }

            var allTenant = await _unitOfWork.TenantReposatory.GetQueryable(filter);

            #endregion

            #region Pagintation

            if (pagination.PageSize <= 0)
                pagination.PageSize = 10;

            if (pagination.PageNumber <= 0)
                pagination.PageNumber = 1;

            var count = await allTenant.CountAsync();

            if (count == 0)
            {
                return new GetResponse<Tenant>
                {
                    isSuccess = false,
                    message = "No data in table",
                    createAt = DateTime.UtcNow,
                };
            }

            var totalPage = (int)Math.Ceiling((double)count / pagination.PageSize);

            var result = await allTenant
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            #endregion

            return new GetResponse<Tenant>
            {
                isSuccess = true,
                message = "All tenant",
                createAt = DateTime.Now,
                Data = result,
                pageSize = pagination.PageSize,
                totalPages = totalPage,
                currentPage = pagination.PageNumber,
                totalTenant = count
            };

        }

        public async Task<AdminResponse> Create(AddTenant addTenant,CancellationToken cancellationToken)
        {
            if (addTenant is null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "enter your failds",
                    createAt = DateTime.UtcNow,
                };
            }

            var exists = await _unitOfWork.TenantReposatory
                .GetAsync(x => x.Email == addTenant.Email);

            if (exists.Any())
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Email already exists",
                    createAt = DateTime.UtcNow,
                };
            }

            var tenant = new Tenant
            {
                Name = addTenant.Name,
                Email = addTenant.Email,
                IsActive = true,
                HaveBranches = addTenant.HaveBranches,
                CreatedAt = DateTime.UtcNow,
            };

            await _unitOfWork.TenantReposatory.CreateAsync(tenant, cancellationToken);

            await _unitOfWork.CommitAsync();

            return new AdminResponse
            {
                isSuccess = true,
                message = "Tenant add successfully",
                createAt = DateTime.UtcNow,
            };
        }

        public async Task<AdminResponse> Update(UpdateTenant updateTenant,int tenantId)
        {
            var tenant = await _unitOfWork.TenantReposatory.GetOneAsync(x=>x.Id== tenantId);

            if(tenant is null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "tenant not found",
                    createAt = DateTime.UtcNow,
                };
            }

            if (!string.IsNullOrEmpty(updateTenant.Name))
            {
                tenant.Name = updateTenant.Name;
            }
            if(!string.IsNullOrEmpty(updateTenant.Email))
            {
                var exists = await _unitOfWork.TenantReposatory
               .GetAsync(x => x.Email == updateTenant.Email && x.Id != tenantId);

                if (exists.Any())
                {
                    return new AdminResponse
                    {
                        isSuccess = false,
                        message = "Email already exists",
                        createAt = DateTime.UtcNow,
                    };
                }

                tenant.Email = updateTenant.Email;
            }
            if (updateTenant.HaveBranches is not null)
            {
                tenant.HaveBranches = updateTenant.HaveBranches.Value;
            }

            await _unitOfWork.CommitAsync();

            return new AdminResponse
            {
                isSuccess = true,
                message = "Update tenant successfully",
                createAt = DateTime.UtcNow,
            };
        }

        public async Task<AdminResponse> Delete(int tenantId)
        {
            var tenant = await _unitOfWork.TenantReposatory.GetOneAsync(s => s.Id == tenantId);

            if(tenant == null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Tentant not found",
                    createAt = DateTime.UtcNow,
                };
            }

            tenant.IsActive = false;

            await _unitOfWork.CommitAsync();

            return new AdminResponse
            {
                isSuccess = true,
                message = "Soft delete from this tenant",
                createAt = DateTime.UtcNow,
            };
        }
    }
}
