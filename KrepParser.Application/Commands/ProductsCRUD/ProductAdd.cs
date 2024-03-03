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
    public record AddProductCommand : BaseProductCommand, IRequest<Result>;

    public sealed class AddProductCommandHandler : IRequestHandler<AddProductCommand, Result>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Result.FailureWithotValue(new Error(ErrorTypes.ValueIsNull, "Не удалось добавить продукт в базу данных."));

            var product = Product.CreateProduct
            (
                request.ProductDTO.Id,
                request.ProductDTO.Name,
                request.ProductDTO.Price,
                request.ProductDTO.ShopName
            );

            //Добавление в бд.
            await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveAndCommitAsync(cancellationToken);

            return Result.SuccessWithoutValue();
        }
    }
}
