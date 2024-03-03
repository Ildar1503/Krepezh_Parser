using KrepParser.Domain.Primitives;

namespace KrepParser.Domain.DomainEvent
{
    public sealed record ProductGetDomainEvent(Guid ProductId) : IDomainEvent;
}
