using KrepParser.Application.Commands.ProductsCRUD;
using KrepParser.Application.DTO;
using KrepParser.Application.Queries;
using KrepParser.Application.Services.Interfaces;
using KrepParser.Domain.Entites;
using KrepParser.Domain.Shared;
using MediatR;

namespace KrepParser.Application.Services.Implementations
{
    public sealed class ProductAppService : IProductAppService
    {
        private readonly ISender _sender;

        public ProductAppService(ISender sender)
        {
            _sender = sender;
        }

        //Добавление продукта.
        public async Task<Result> AddProductAsync(AddProductCommand product, CancellationToken cancellationToken)
        {
            return await _sender.Send(product, cancellationToken);
        }

        //Удаление продукта.
        public async Task<Result> DeleteProductAsync(DeleteProductCommand product, CancellationToken cancellationToken)
        {
            return await _sender.Send(product, cancellationToken);
        }

        //Обновление продукта.
        public async Task<Result> UpdateProductAsync(UpdateProductCommand product, CancellationToken cancellationToken)
        {
            return await _sender.Send(product, cancellationToken);
        }

        //Получение продуктов из магазина, содержащий в названии - наименование продукта.
        public async Task<Result<IEnumerable<Product>>> GetCurrentShopProductsByNameAsync(GetCurrentShopProductsByNameQuery query, CancellationToken cancellationToken)
        {
            return await _sender.Send(query, cancellationToken);
        }

        //Получение продукта по названию.
        public async Task<Result<Product>> GetProductByNameAsync(GetProductsByNameQuery query, CancellationToken cancellationToken)
        {
            return await _sender.Send(query, cancellationToken);
        }
    }
}
