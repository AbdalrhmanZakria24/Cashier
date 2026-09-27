using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cashier.Application.CQRS.Query.Product.GetProducts
{
    public class GetProductsQueryHandler: IRequestHandler<GetProductsQuery, ResultT<ProductResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<ProductResponse>> Handle(GetProductsQuery request,CancellationToken cancellationToken)
        {
            #region Search
            var product = request.Product;

            Expression<Func<Cashier.Domain.Entities.Product, bool>>? filter = null;

            if (product != null)
            {
                filter = f =>
                    (string.IsNullOrEmpty(product.Name) ||
                     f.Name.Contains(product.Name)) &&

                    (product.CategoryId == 0 ||
                     f.CategoryId == product.CategoryId) &&

                    (product.IsActive == null ||
                     f.IsActive == product.IsActive.Value) &&

                    (product.MinPrice <= 0 ||
                     f.Price >= product.MinPrice) &&

                    (product.MaxPrice <= 0 ||
                     f.Price <= product.MaxPrice);
            }

            var allProducts =await _unitOfWork.ProductReposatory
                .GetQueryable(filter, Tracking: false);

            #endregion
            #region Pagination
            var count = await allProducts.CountAsync(cancellationToken);

            if (count <= 0)
            {
                return ResultT<ProductResponse>.Success(
                    new ProductResponse
                    {
                        Data = { },
                        totalCount = 0,
                        totalPages = 0,
                        currentPage = request.Pagination.PageNumber
                    });
            }

            var totalPages =
                (int)Math.Ceiling((double)count / request.Pagination.PageSize);

            var result = await allProducts
                .OrderBy(x => x.Id)
                .Skip((request.Pagination.PageNumber - 1) * request.Pagination.PageSize)
                .Take(request.Pagination.PageSize)
                .ToListAsync(cancellationToken);
            #endregion
            var response = new ProductResponse
            {
                Data = result,
                totalCount = count,
                totalPages = totalPages,
                currentPage = request.Pagination.PageNumber
            };

            return ResultT<ProductResponse>.Success(response);
        }
    }
}