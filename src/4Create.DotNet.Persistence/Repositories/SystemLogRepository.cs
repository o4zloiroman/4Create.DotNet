using System.Data;
using _4Create.DotNet.Persistence.Records;
using Dapper;

namespace _4Create.DotNet.Persistence.Repositories;

public sealed class SystemLogRepository : ISystemLogRepository
{
    private readonly IDbConnection _connection;

    public SystemLogRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task InsertAsync(SystemLogRecord log, CancellationToken ct)
    {
        await _connection.ExecuteAsync(new CommandDefinition(
            SystemLogQueries.InsertQuery, 
            new
            {
                resourceType = log.ResourceType,
                createdAt = log.CreatedAt,
                @event = log.Event,
                changeset = log.Changeset,
                comment = log.Comment
            },
            cancellationToken: ct
        ));
    }
}

internal static class SystemLogQueries
{
    internal const string InsertQuery = @"
INSERT INTO SystemLog (resource_type, created_at, event, changeset, comment) 
VALUES (@resourceType, @createdAt, @event, @changeset, @comment);
";
}