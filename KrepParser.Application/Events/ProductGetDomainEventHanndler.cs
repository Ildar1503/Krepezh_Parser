using KrepParser.Domain.DomainEvent;
using KrepParser.Domain.Repositories;
using MediatR;

namespace KrepParser.Application.Events
{
    /// <summary>
    /// Обработчик доменного события.
    /// </summary>
    public sealed class ProductGetDomainEventHanndler
        : INotificationHandler<ProductGetDomainEvent>
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Получение объекта по id и его проверка на null. 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(ProductGetDomainEvent notification, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(notification.ProductId, cancellationToken);

            if (product is null)
            {
                return;
            }

            //TODO:По возможности добавить какое-то действие, которое совершается после удачного получения продукта.
        }
    }
}
