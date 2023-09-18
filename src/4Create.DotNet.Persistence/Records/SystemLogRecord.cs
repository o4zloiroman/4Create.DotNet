namespace _4Create.DotNet.Persistence.Records;

public sealed record SystemLogRecord
{
    public string ResourceType { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Event { get; init; }
    public object Changeset { get; init; }
    public string Comment { get; init; }
}