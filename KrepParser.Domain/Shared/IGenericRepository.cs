using KrepParser.Domain.Primitives;

namespace KrepParser.Domain.Shared
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        #region CRUD

        public Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public void Update(TEntity entity, CancellationToken cancellationToken);

        #endregion

        #region Другие операции

        public Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        public Task<List<TEntity>> GetByNameAsync(string name, CancellationToken cancellationToken);
        public Task<TEntity> GetByNameAndShopNameAsync(string name, string shopName, CancellationToken cancellationToken);    

        #endregion
    }
}
