namespace KrepParser.Domain.Primitives
{
    /// <summary>
    /// Реализация паттерна аггрегированного пути.
    /// </summary>
    public abstract class AggregateRoot : Entity
    {
        //Для записи событий домена
        private readonly List<IDomainEvent> _domainEventList;

        protected AggregateRoot(Guid id) : base(id)
        { }

        protected AggregateRoot()
        { }

        //Метод для записи данных о доменных событиях.
        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEventList.Add(domainEvent);
        }
    }
}
