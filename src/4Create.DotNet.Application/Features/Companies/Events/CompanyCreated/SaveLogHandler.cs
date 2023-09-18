using System.Text.Json;
using _4Create.DotNet.Domain.Companies;
using _4Create.DotNet.Persistence.Records;
using _4Create.DotNet.Persistence.Repositories;
using MediatR;

namespace _4Create.DotNet.Application.Features.Companies.Events.CompanyCreated;

public sealed class SaveLogHandler : INotificationHandler<CompanyCreatedEvent>
{
    private readonly ISystemLogRepository _systemLogRepository;

    public SaveLogHandler(ISystemLogRepository systemLogRepository)
    {
        _systemLogRepository = systemLogRepository;
    }

    public async Task Handle(CompanyCreatedEvent notification, CancellationToken ct)
    {
        var record = new SystemLogRecord
        {
            ResourceType = nameof(notification.Company),
            CreatedAt = DateTime.UtcNow,
            Event = notification.GetType().Name,
            Changeset = JsonSerializer.Serialize(notification.Company),
            Comment = $"New Company {notification.Company.Name} was created"
        }; 
        await _systemLogRepository.InsertAsync(record, ct);
    }
}