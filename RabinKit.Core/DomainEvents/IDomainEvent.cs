using MediatR;

namespace RabinKit.Core.DomainEvents
{
    /// <summary>
    /// Доменное событие
    /// </summary>
    public interface IDomainEvent : INotification;
}
