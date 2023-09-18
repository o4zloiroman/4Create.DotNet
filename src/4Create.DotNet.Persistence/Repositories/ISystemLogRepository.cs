using _4Create.DotNet.Persistence.Records;

namespace _4Create.DotNet.Persistence.Repositories;

public interface ISystemLogRepository
{
    Task InsertAsync(SystemLogRecord log, CancellationToken ct);
}