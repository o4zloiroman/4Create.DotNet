namespace _4Create.DotNet.Persistence.Records;

public sealed class CompanyRecord
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Guid> EmployeeIds { get; set; }
    public DateTime CreatedAt { get; set; }
}