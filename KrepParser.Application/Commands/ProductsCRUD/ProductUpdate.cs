using KrepParser.Application.Commands.Primitves;
using KrepParser.Application.DTO;
using KrepParser.Application.Shared;
using KrepParser.Domain.Entites;
using KrepParser.Domain.Enums;
using KrepParser.Domain.Repositories;
using KrepParser.Domain.Shared;
using MediatR;

namespace KrepParser.Application.Commands.ProductsCRUD
{
    public record UpdateProductCommand : BaseProductCommand, IRequest<Result>
    {
        public ProductDTO NewProductDTO { get; set; }
    }

    public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (request.ProductDTO == null)
                return Result.FailureWithotValue(new Error(ErrorTypes.ValueIsNull, "Объект пуст!"));

            if (request.NewProductDTO.ShopName == null || request.NewProductDTO.Name == null || request.NewProductDTO.Price < 0)
                return Result.FailureWithotValue(new Error(ErrorTypes.ValueIsNull, "Проверьте введенные данные!"));

            //Получение продукта.
            var productList = await _productRepository.GetAllAsync(cancellationToken);
            var currentProduct = productList
                .FirstOrDefault(product => product.Name == request.ProductDTO.Name 
                && product.ShopName == request.ProductDTO.ShopName);

            if (currentProduct is null)
                return Result.FailureWithotValue(new Error(ErrorTypes.ValueIsNotFound, "Продукт не найден!"));

            var product = Product.CreateProduct
                (currentProduct.Id, 
                request.NewProductDTO.Name,
                request.NewProductDTO.Price,
                request.NewProductDTO.ShopName);

            _productRepository.Update(product, cancellationToken);
            await _unitOfWork.SaveAndCommitAsync(cancellationToken);

            return Result.SuccessWithoutValue();
        }
    }
}
