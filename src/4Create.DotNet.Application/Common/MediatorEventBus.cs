using _4Create.DotNet.Domain.Common;
using MediatR;

namespace _4Create.DotNet.Application.Common;

public sealed class MediatorEventBus : IEventBus
{
    private readonly IMediator _mediator;

    public MediatorEventBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task PublishAsync(IDomainEvent e, CancellationToken cancellationToken)
    {
        return _mediator.Publish(e, cancellationToken);
    }

    public async Task PublishAllAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken)
    {
        foreach (var e in events)
        {
            await PublishAsync(e, cancellationToken);
        }
    }
}