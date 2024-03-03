using KrepParser.Domain.Entites;
using KrepParser.Domain.Repositories;
using KrepParser.Domain.ValueObjects;
using KrepParser.Infrastruction.DataBase;
using Microsoft.EntityFrameworkCore;

namespace KrepParser.Infrastruction.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _productDbContext;

        public ProductRepository(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task AddAsync(Product entity, CancellationToken cancellationToken)
        {
            if (entity is not null)
            {
                await _productDbContext.AddAsync(entity, cancellationToken);
            }
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productDbContext.Products.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
            if (product is not null)
            {
                _productDbContext.Products.Remove(product);
                return true;
            }

            return false;
        }

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _productDbContext.Products.ToListAsync(cancellationToken);
        }

        public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _productDbContext.Products.FirstOrDefaultAsync(i => i.Id == id, cancellationToken); 
        }

        public async Task<Product> GetByNameAndShopNameAsync(string name, string shopName, CancellationToken cancellationToken)
        {
            return await _productDbContext.Products
                .Where(product => product.Name == name && product.ShopName == shopName).FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Получение товаров с таким же названием, либо содержащих его.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Product>> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            var productWithCurrentName = await _productDbContext.Products
                .AsNoTracking()
                .Where(i => i.Name == name || i.Name.Contains(name))
                .ToListAsync(cancellationToken);

            return productWithCurrentName;
        }

        public async Task<double> GetPriceCurrentProduct(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productDbContext.Products.FirstOrDefaultAsync(i => i.Id == id);

            if (product is not null)
            {
                return product.Price;
            }

            return 0;
        }

        //TODO: Подумать над реализацией.
        public Task<IEnumerable<Product>> GetPricesProductCollection(IEnumerable<Product> products, CancellationToken cancellationToken)
        {
            return Task.FromResult(products);
            //var productsPrice = products.Select(i => i.Price).ToList(); 
        }

        public void Update(Product entity, CancellationToken cancellationToken)
        {
            _productDbContext.Update(entity);
        }
    }
}
