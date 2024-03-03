using KrepParser.Application.Queries.Primitives;
using KrepParser.Application.Shared;
using KrepParser.Domain.Entites;
using KrepParser.Domain.Enums;
using KrepParser.Domain.Repositories;
using KrepParser.Domain.Shared;
using MediatR;

namespace KrepParser.Application.Queries
{
    public sealed record GetCurrentShopProductsByNameQuery : BaseProductQuery, IRequest<Result<IEnumerable<Product>>>;

    public sealed class GetCurrentShopProductsByNameQueryHandeler
        : IRequestHandler<GetCurrentShopProductsByNameQuery, Result<IEnumerable<Product>>>
    {
        private readonly IProductRepository _productRepository;

        public GetCurrentShopProductsByNameQueryHandeler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<IEnumerable<Product>>> Handle(GetCurrentShopProductsByNameQuery request, CancellationToken cancellationToken)
        {
            if (request is null)
                return Result<IEnumerable<Product>>.Failure
                    (new List<Product>() { Product.GetEmptyProduct() }, new Error(ErrorTypes.ValueIsNull, QueriesResultMessage.QueryIsNull));

            if (request.Name == null || request.ShopName == null)
                return Result<IEnumerable<Product>>.Failure
                    (new List<Product>() { Product.GetEmptyProduct() }, new Error(ErrorTypes.ValueIsNull, QueriesResultMessage.QueryDataIsNull));

            var allProducts = await _productRepository.GetAllAsync(cancellationToken);
            var products = allProducts.Where(product => product.Name.ToLower().Contains(request.Name.ToLower()) 
                && product.ShopName == request.ShopName);

            if (products is null)
                return Result<IEnumerable<Product>>.Failure
                    (new List<Product>() { Product.GetEmptyProduct() }, new Error(ErrorTypes.ValueIsNull, QueriesResultMessage.QueryDataIsNotFound));

            return Result<IEnumerable<Product>>.Succes(products);
        }
    }
}
