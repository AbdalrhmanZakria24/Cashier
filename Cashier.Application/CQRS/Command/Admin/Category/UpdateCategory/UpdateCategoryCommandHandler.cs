using MediatR;

namespace Cashier.Application.CQRS.Command.Category.UpdateCategory
{
    public class UpdateCategoryCommandHandler
        : IRequestHandler<UpdateCategoryCommand, ResultT<long>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<long>> Handle(
            UpdateCategoryCommand request,
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

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                category.Name = request.Name;
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return ResultT<long>.Success(category.Id);
        }
    }
}