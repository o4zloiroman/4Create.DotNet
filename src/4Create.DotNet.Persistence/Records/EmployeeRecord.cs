namespace _4Create.DotNet.Persistence.Records;

public sealed record EmployeeRecord
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public EmployeeTitle Title { get; set; }
    public IEnumerable<Guid> CompanyIds { get; set; }
    public DateTime CreatedAt { get; set; }
}