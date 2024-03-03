namespace KrepParser.Application.Shared
{
    /// <summary>
    /// Интерфейс для паттерна unit of work
    /// </summary>
    public interface IUnitOfWork
    {
        public Task SaveAndCommitAsync(CancellationToken cancellationToken);
    }
}
