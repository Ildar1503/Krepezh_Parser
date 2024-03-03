using KrepParser.Application.Commands.Primitves;
using KrepParser.Application.Shared;
using KrepParser.Domain.Enums;
using KrepParser.Domain.Repositories;
using KrepParser.Domain.Shared;
using MediatR;

namespace KrepParser.Application.Commands.ProductsCRUD
{
    public record DeleteProductCommand : BaseProductCommand, IRequest<Result>;

    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (request.ProductDTO == null)
                return Result.FailureWithotValue(new Error(ErrorTypes.ValueIsNull, "Объект пуст!"));

            if (request.ProductDTO.ShopName == null || request.ProductDTO.Name == null)
                return Result.FailureWithotValue(new Error(ErrorTypes.ValueIsNull, "Одно из обязательных полей пусто!"));

            //Получение продукта.
            var productList = await _productRepository.GetAllAsync(cancellationToken);
            var currentProduct = productList
                .FirstOrDefault(product => product.Name == request.ProductDTO.Name
                && product.ShopName == request.ProductDTO.ShopName);

            if (currentProduct is null)
                return Result.FailureWithotValue(new Error(ErrorTypes.ValueIsNotFound, "Продукт не найден!"));

            var deleteResult = await _productRepository.DeleteAsync(currentProduct.Id, cancellationToken);

            if (deleteResult)
                return Result.SuccessWithoutValue();

            return Result.FailureWithotValue(new Error(ErrorTypes.OperationNotCompleted, "Не получилось удалить продукт!"));
        }
    }
}
