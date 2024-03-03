using KrepParser.Application.Queries.Primitives;
using KrepParser.Application.Shared;
using KrepParser.Domain.Entites;
using KrepParser.Domain.Enums;
using KrepParser.Domain.Repositories;
using KrepParser.Domain.Shared;
using MediatR;

namespace KrepParser.Application.Queries
{
    public sealed record GetProductsByNameQuery : BaseProductQuery, IRequest<Result<Product>>;

    public sealed class GetProductsByNameQueryHandler : IRequestHandler<GetProductsByNameQuery, Result<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByNameQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<Product>> Handle
            (GetProductsByNameQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                return Result<Product>.Failure
                    (Product.GetEmptyProduct(), new Error(ErrorTypes.ValueIsNull, QueriesResultMessage.QueryIsNull));

            if (request.Name == null || request.ShopName == null)
                return Result<Product>.Failure
                    (Product.GetEmptyProduct(), new Error(ErrorTypes.ValueIsNull, QueriesResultMessage.QueryDataIsNull));

            var product = await _productRepository.GetByNameAndShopNameAsync(request.Name, request.ShopName, cancellationToken);

            if (product is null)
                return Result<Product>.Failure
                    (Product.GetEmptyProduct(), new Error(ErrorTypes.ValueIsNull, QueriesResultMessage.QueryDataIsNotFound));

            return Result<Product>.Succes(product);
        }
    }
}
