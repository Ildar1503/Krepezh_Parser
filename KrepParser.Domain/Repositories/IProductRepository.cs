using KrepParser.Domain.Entites;
using KrepParser.Domain.Shared;

namespace KrepParser.Domain.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<double> GetPriceCurrentProduct(Guid id, CancellationToken cancellationToken);
        public Task<IEnumerable<Product>> GetPricesProductCollection(IEnumerable<Product> products, CancellationToken cancellationToken);
    }
}
