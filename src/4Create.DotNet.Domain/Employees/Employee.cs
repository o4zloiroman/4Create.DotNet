using _4Create.DotNet.Domain.Common;

namespace _4Create.DotNet.Domain.Employees;

public sealed class Employee : Aggregate<Guid>
{
    public string Email { get; private set; }
    public EmployeeTitle Title { get; private set; }
    public IReadOnlyList<Guid> CompanyIds => _companyIds;
    private readonly List<Guid> _companyIds;
    
    public Employee(Guid id, string email, EmployeeTitle title, List<Guid> companyIds) : base(id)
    {
        Email = email;
        Title = title;
        _companyIds = companyIds;
    }

    public static Employee Create(string email, EmployeeTitle title, IEnumerable<Guid> companyIds)
    {
        var employee = new Employee(Guid.NewGuid(), email, title, companyIds.ToList());
        employee.AddEvent(new EmployeeCreatedEvent(employee));

        return employee;
    }
    
    public static Employee Create(string email, EmployeeTitle title, Guid companyId)
    {
        var employee = new Employee(Guid.NewGuid(), email, title, new List<Guid>{ companyId });
        employee.AddEvent(new EmployeeCreatedEvent(employee));

        return employee;
    }
}