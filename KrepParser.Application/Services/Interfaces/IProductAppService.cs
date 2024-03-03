using KrepParser.Application.Commands.ProductsCRUD;
using KrepParser.Application.Queries;
using KrepParser.Domain.Entites;
using KrepParser.Domain.Shared;

namespace KrepParser.Application.Services.Interfaces
{
    public interface IProductAppService
    {
        #region CRUD Product

        //Добавление товара в бд.
        public Task<Result> AddProductAsync(AddProductCommand product, CancellationToken cancellationToken);

        //Обновление данных о товаре.
        public Task<Result> UpdateProductAsync(UpdateProductCommand product, CancellationToken cancellationToken);

        //Удаление продукта.
        public Task<Result> DeleteProductAsync(DeleteProductCommand product, CancellationToken cancellationToken);

        #endregion

        //Данные для вывода.
        #region CRUD ProductDTO
        #endregion

        #region Другие операции.

        public Task<Result<Product>> GetProductByNameAsync(GetProductsByNameQuery query, CancellationToken cancellationToken);
        public Task<Result<IEnumerable<Product>>> GetCurrentShopProductsByNameAsync(GetCurrentShopProductsByNameQuery query, CancellationToken cancellationToken);

        #endregion
    }
}
