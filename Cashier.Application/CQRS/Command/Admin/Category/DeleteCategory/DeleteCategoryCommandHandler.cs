using MediatR;

namespace Cashier.Application.CQRS.Command.Category.DeleteCategory
{
    public class DeleteCategoryCommandHandler
        : IRequestHandler<DeleteCategoryCommand, ResultT<long>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<long>> Handle(
            DeleteCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category =
                await _unitOfWork.Categoryreposatory
                    .GetOneAsync(
                        x => x.Id == request.Id,
                        Tracking: true);

            if (category is null)
            {
                return ResultT<long>.Failure(
                    new Error(
                        "Category.NotFound",
                        "Category not found",
                        ErrorType.NotFound));
            }

            _unitOfWork.Categoryreposatory.Remove(category);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ResultT<long>.Success(category.Id);
        }
    }
}