using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cashier.Application.CQRS.Query.Category.GetCategories
{
    public class GetCategoriesQueryHandler: IRequestHandler<GetCategoriesQuery,ResultT<CategoryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<CategoryResponse>> Handle(
            GetCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Cashier.Domain.Entities.Category, bool>>? filter = null;

                filter = x =>
                    (string.IsNullOrEmpty(request.Name)
                        || x.Name.Contains(request.Name));
            

            var allCategories =
                await _unitOfWork.Categoryreposatory
                    .GetQueryable(
                        filter,
                        Tracking: false);

            var totalCategory = await allCategories.CountAsync(
                cancellationToken);

            if (totalCategory == 0)
            {
                return ResultT<CategoryResponse>.Success(
                    new CategoryResponse
                    {
                        Data = { },
                        totalCount = 0,
                        totalPages = 0,
                        currentPage = request.Pagination.PageNumber,
                        pageSize = request.Pagination.PageSize
                    });
            }

            var totalPage = (int)Math.Ceiling(
                (double)totalCategory /
                request.Pagination.PageSize);

            var result = await allCategories
                .Skip(
                    (request.Pagination.PageNumber - 1)
                    * request.Pagination.PageSize)
                .Take(request.Pagination.PageSize)
                .ToListAsync(cancellationToken);

            var response = new CategoryResponse
            {
                Data = result,
                totalCount = totalCategory,
                totalPages = totalPage,
                currentPage = request.Pagination.PageNumber,
                pageSize = request.Pagination.PageSize
            };

            return ResultT<CategoryResponse>.Success(response);
        }
    }
}