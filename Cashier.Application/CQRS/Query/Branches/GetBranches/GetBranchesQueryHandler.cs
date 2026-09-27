using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cashier.Application.CQRS.Query.Branch.GetBranches
{
    public class GetBranchesQueryHandler
        : IRequestHandler<
            GetBranchesQuery,
            ResultT<BranshResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBranchesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<BranshResponse>> Handle(
            GetBranchesQuery request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Cashier.Domain.Entities.Branch, bool>>? filter = null;

                filter = x =>
                    (string.IsNullOrEmpty(request.BranchSerc.Name)
                        || x.Name.Contains(request.BranchSerc.Name))&&

                    (string.IsNullOrEmpty(request.BranchSerc.Address)
                        || x.Address.Contains(request.BranchSerc.Address))&&

                    (request.BranchSerc.IsActive == null
                        || x.IsActive == request.BranchSerc.IsActive);
            

            var allBranch =
                await _unitOfWork.BranchReposatory.GetQueryable(
                    filter);

            var count = await allBranch.CountAsync(
                cancellationToken);

            if (count == 0)
            {
                return ResultT<BranshResponse>.Success(
                    new BranshResponse
                    {
                        Data = {},
                        totalCount = 0,
                        totalPages = 0,
                        currentPage = request.Pagination.PageNumber,
                        pageSize = request.Pagination.PageSize
                    });
            }

            var totalPage = (int)Math.Ceiling(
                (double)count /
                request.Pagination.PageSize);

            var result = await allBranch
                .Skip(
                    (request.Pagination.PageNumber - 1)
                    * request.Pagination.PageSize)
                .Take(request.Pagination.PageSize)
                .ToListAsync(cancellationToken);

            var response = new BranshResponse
            {
                Data = result,
                totalPages = totalPage,
                totalCount = count,
                pageSize = request.Pagination.PageSize,
                currentPage = request.Pagination.PageNumber
            };

            return ResultT<BranshResponse>.Success(response);
        }
    }
}