using MediatR;

namespace Cashier.Application.CQRS.Command.Category.CreateCategory
{
    public class CreateCategoryCommandHandler
        : IRequestHandler<CreateCategoryCommand, ResultT<long>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultT<long>> Handle(
            CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category =
                await _unitOfWork.Categoryreposatory
                    .GetOneAsync(x =>
                        x.Name == request.Name);

            if (category is not null)
            {
                return ResultT<long>.Failure(
                    new Error(
                        "Category.AlreadyExists",
                        "Category already exists",
                        ErrorType.BadRequest));
            }

            var newCategory = new Cashier.Domain.Entities.Category
            {
                Name = request.Name,
            };

            await _unitOfWork.Categoryreposatory
                .CreateAsync(
                    newCategory,
                    cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ResultT<long>.Success(newCategory.Id);
        }
    }
}