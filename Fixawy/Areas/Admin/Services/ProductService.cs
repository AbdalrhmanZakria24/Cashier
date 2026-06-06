using Fixawy.DataAccess.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fixawy.Areas.Admin.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetResponse<Product>> Get(ProductSearch? product, Pagination pagination)
        {
            #region Search

            Expression<Func<Product, bool>>? filter = null;

            if (product != null)
            {
                if (product.MinPrice > product.MaxPrice)
                {
                    return new GetResponse<Product>
                    {
                        isSuccess = false,
                        message = "Bad request",
                        createAt = DateTime.UtcNow,
                    };
                }

                filter = f =>
                (string.IsNullOrEmpty(product.Name) || f.Name.Contains(product.Name)) &&
                (product.CategoryId == 0 || f.CategoryId == product.CategoryId) &&
                (product.TenantId == 0 || f.TenantId == product.TenantId) &&
                (product.IsActive == null || f.IsActive == product.IsActive.Value) &&
                (product.MinPrice <= 0 || f.Price >= product.MinPrice) &&
                (product.MaxPrice <= 0 || f.Price <= product.MaxPrice);
            }

            var allProduct = await _unitOfWork.ProductReposatory.GetQueryable(filter, Tracking: false);

            #endregion
            #region Pagination

            if (pagination.PageNumber <= 0)
                pagination.PageNumber = 1;

            if (pagination.PageSize <= 0)
                pagination.PageSize = 10;

            var count = await allProduct.CountAsync();

            if (count <= 0)
            {
                return new GetResponse<Product>
                {
                    isSuccess = true,
                    message = "no data found",
                    createAt = DateTime.UtcNow,
                };
            }

            var allPage = (int)Math.Ceiling((double)count / pagination.PageSize);

            var result = await allProduct
                .OrderBy(x => x.Id)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new GetResponse<Product>
            {
                isSuccess = true,
                message = "All product",
                createAt = DateTime.UtcNow,
                Data = result,
                totalCount = count,
                totalPages = allPage,
                currentPage = pagination.PageNumber,
            };

            #endregion
        }

        public async Task<AdminResponse> Create(AddProduct addProduct, CancellationToken cancellationToken)
        {
            var tenant = await _unitOfWork.TenantReposatory.GetOneAsync(c => c.Id == addProduct.TenantId);
            if (tenant == null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "no tenant found",
                    createAt = DateTime.UtcNow,
                };
            }

            var category = await _unitOfWork.Categoryreposatory.GetOneAsync(c => c.Id == addProduct.CategoryId);
            if (category == null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "no category found",
                    createAt = DateTime.UtcNow,
                };
            }

            var product = await _unitOfWork.ProductReposatory
                .GetOneAsync(x => x.Name == addProduct.Name &&
                x.TenantId == addProduct.TenantId &&
                x.CategoryId == addProduct.CategoryId);

            //await using to whene scope end user await transaction.DisposeAsync() not transaction.Dispose();

            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (product == null)
                {
                    product = new Product
                    {
                        Name = addProduct.Name,
                        Price = addProduct.Price,
                        Cost = addProduct.Cost,
                        TenantId = addProduct.TenantId,
                        CategoryId = addProduct.CategoryId,
                        IsActive = addProduct.IsActive,
                        Version = addProduct.Version,
                    };

                    await _unitOfWork.ProductReposatory.CreateAsync(product, cancellationToken);
                }
                await _unitOfWork.CommitAsync();

                var branchIds = addProduct.AddProductBranches.Select(c => c.BranchId).ToList();

                var existingProducts = await _unitOfWork.BranchProductReposatory
                    .GetAsync(c => c.ProductId == product!.Id && branchIds.Contains(c.BranchId));

                var lookup = existingProducts.ToDictionary(x => x.BranchId);

                foreach (var branchDto in addProduct.AddProductBranches)
                {
                    if (lookup.TryGetValue(branchDto.BranchId, out var existing))
                    {
                        existing.Quantity += branchDto.Quantity;
                    }
                    else
                    {
                        await _unitOfWork.BranchProductReposatory.CreateAsync(
                            new BranchProduct
                            {
                                ProductId = product!.Id,
                                BranchId = branchDto.BranchId,
                                Quantity = branchDto.Quantity
                            },
                            cancellationToken
                        );
                    }
                }

                await _unitOfWork.CommitAsync();
                await transaction.CommitAsync();

                return new AdminResponse
                {
                    isSuccess = true,
                    message = "Product add successfully",
                    createAt = DateTime.UtcNow,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new AdminResponse
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }

        public async Task<AdminResponse> Update(UpdateProduct updateProduct, long id, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductReposatory.GetOneAsync(c => c.Id == id);

            if (product == null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Can not found the product",
                    createAt = DateTime.UtcNow,
                };
            }
            if (!string.IsNullOrEmpty(updateProduct.Name))
                product.Name = updateProduct.Name;

            if (updateProduct.Price.HasValue)
                product.Price = updateProduct.Price.Value;

            if (updateProduct.Cost.HasValue)
                product.Cost = updateProduct.Cost.Value;

            if (updateProduct.IsActive.HasValue)
                product.IsActive = updateProduct.IsActive.Value;

            if (updateProduct.AddProductBranches is not null)
            {
                var BranchProduct = await _unitOfWork.BranchProductReposatory.GetAsync(c => c.ProductId == id);

                var lookup = BranchProduct.ToDictionary(x => x.BranchId);

                foreach (var i in updateProduct.AddProductBranches)
                {
                    if (lookup.TryGetValue(i.BranchId, out var existing))
                    {
                        existing.Quantity = i.Quantity;
                    }
                    else
                    {
                        var branchProduct = new BranchProduct
                        {
                            ProductId = id,
                            BranchId = i.BranchId,
                            Quantity = i.Quantity,
                        };

                        await _unitOfWork.BranchProductReposatory.CreateAsync(branchProduct, cancellationToken);
                    }
                }
            }

            await _unitOfWork.CommitAsync();

            return new AdminResponse
            {
                isSuccess = true,
                message = "Update product Successfully",
                createAt = DateTime.UtcNow,
            };

        }

        public async Task<AdminResponse> Delete(long id, long branchId)
        {
            var Branchproduct = await _unitOfWork.BranchProductReposatory
                .GetOneAsync(x => x.ProductId == id && x.BranchId == branchId);

            if (Branchproduct is null)
            {
                return new AdminResponse
                {
                    isSuccess = false,
                    message = "Product not found",
                    createAt = DateTime.UtcNow,
                };
            }

            Branchproduct.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return new AdminResponse
            {
                isSuccess = true,
                message = "Product removed from branch successfully",
                createAt = DateTime.UtcNow,
            };
        }
    }
}
