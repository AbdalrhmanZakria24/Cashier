using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fixawy.Areas.Admin.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<GetResponse<CategoryDto>> Get(CategorySerch? categorySerch, Pagination pagination)
        {
            #region Search
            Expression<Func<Category, bool>>? filter = null;

            if (categorySerch is not null)
            {
                filter = f =>
                (string.IsNullOrEmpty(categorySerch.Name) || f.Name.Contains(categorySerch.Name)) &&
                (categorySerch.TenantId == null || f.TenantId == categorySerch.TenantId);
            }

            var allCategories = await _unitOfWork.Categoryreposatory
                .GetQueryable(filter, Tracking: false);
            #endregion
            #region Pagination
            if (pagination.PageNumber <= 0)
                pagination.PageNumber = 1;

            if (pagination.PageSize <= 0)
                pagination.PageSize = 10;

            var totalCategory = await allCategories.CountAsync();

            if (totalCategory == 0)
            {
                return new GetResponse<CategoryDto>
                {
                    isSuccess = true,
                    message = "No data found",
                    createAt = DateTime.UtcNow,
                };
            }

            var totalPage = (int)Math.Ceiling((double)totalCategory / pagination.PageSize);

            var result = await allCategories
                  .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                  .Take(pagination.PageSize)
                  .Select(n => new CategoryDto
                  {
                      Id = n.Id,
                      Name = n.Name,
                      TenantName = n.Tenants.Name,
                      TenantEmail = n.Tenants.Email
                  })
                  .ToListAsync();

            #endregion

            return new GetResponse<CategoryDto>
            {
                isSuccess = true,
                message = "all categories",
                createAt = DateTime.UtcNow,
                Data = result,
                totalCount = totalCategory,
                totalPages = totalPage,
                currentPage = pagination.PageNumber,
                pageSize = pagination.PageSize
            };

        }

        public async Task<AdminResponse> Create(AddCategory addCategory, CancellationToken cancellationToken)
        {

            if (addCategory.TenantId <= 0)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "enter tenant id",
                    createAt = DateTime.UtcNow,
                };
            }

            var category = await _unitOfWork.Categoryreposatory
                .GetOneAsync(c => c.TenantId == addCategory.TenantId && c.Name == addCategory.Name);

            if (category is not null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Category is already exit",
                    createAt = DateTime.UtcNow,
                };
            }

            if (string.IsNullOrEmpty(addCategory.Name))
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "enter category name",
                    createAt = DateTime.UtcNow,
                };
            }
            var newCategory = new Category
            {
                Name = addCategory.Name,
                TenantId = addCategory.TenantId,
            };

            await _unitOfWork.Categoryreposatory.CreateAsync(newCategory, cancellationToken);

            await _unitOfWork.CommitAsync();

            return new AdminResponse
            {
                isSuccess = true,
                message = "Created category successfully",
                createAt = DateTime.UtcNow,
            };
        }

        public async Task<AdminResponse> Update(UpdateCategory updateCategory ,long id)
        {
            if(id == 0)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "id is  wrong",
                    createAt = DateTime.UtcNow,
                };
            }

            var ctaegory =await _unitOfWork.Categoryreposatory.GetOneAsync(c=>c.Id == id ,Tracking: true);

            if(ctaegory == null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Cannot find category",
                    createAt = DateTime.UtcNow,
                };
            }

            if (!string.IsNullOrEmpty(updateCategory.Name))
            {
                ctaegory.Name = updateCategory.Name!;
            }

            await _unitOfWork.CommitAsync();

            return new AdminResponse
            {
                isSuccess = true,
                message = "updated successfully",
                createAt = DateTime.UtcNow,
            };
        }

        public async Task<AdminResponse> Delete (long id)
        {
            if (id == 0)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "id is wrong",
                    createAt = DateTime.UtcNow,
                };
            }

            var ctaegory = await _unitOfWork.Categoryreposatory.GetOneAsync(c => c.Id == id, Tracking: true);

            if (ctaegory == null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Cannot find category",
                    createAt = DateTime.UtcNow,
                };
            }

            _unitOfWork.Categoryreposatory.Remove(ctaegory);
            await _unitOfWork.CommitAsync();

            return new AdminResponse
            {
                isSuccess = true,
                message = "Deleted successfully",
                createAt = DateTime.UtcNow,
            };

        }
    }
}
