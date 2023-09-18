using System.Text.Json;
using _4Create.DotNet.Domain.Employees;
using _4Create.DotNet.Persistence.Records;
using _4Create.DotNet.Persistence.Repositories;
using MediatR;

namespace _4Create.DotNet.Application.Features.Employees.Events.EmployeeCreated;

public sealed class SaveLogHandler : INotificationHandler<EmployeeCreatedEvent>
{
    private readonly ISystemLogRepository _systemLogRepository;

    public SaveLogHandler(ISystemLogRepository systemLogRepository)
    {
        _systemLogRepository = systemLogRepository;
    }

    public async Task Handle(EmployeeCreatedEvent notification, CancellationToken ct)
    {
        var record = new SystemLogRecord
        {
            ResourceType = nameof(notification.Employee),
            CreatedAt = DateTime.UtcNow,
            Event = notification.GetType().Name,
            Changeset = JsonSerializer.Serialize(notification.Employee),
            Comment = $"New employee {notification.Employee.Email} was created"
        }; 
        await _systemLogRepository.InsertAsync(record, ct);
    }
}