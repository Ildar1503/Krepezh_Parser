using KrepParser.Application.Shared;

namespace KrepParser.Infrastruction.DataBase
{
    /// <summary>
    /// Реализация интерфейса IUnitOfWork
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductDbContext _productDbContext;

        public UnitOfWork(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task SaveAndCommitAsync(CancellationToken cancellationToken)
        {
            await _productDbContext.SaveChangesAsync();
        }
    }
}
