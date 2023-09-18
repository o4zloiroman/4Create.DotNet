namespace _4Create.DotNet.Domain.Common;

public interface IEventBus
{
    Task PublishAsync(IDomainEvent e, CancellationToken cancellationToken);
    Task PublishAllAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken);
}