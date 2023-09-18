using _4Create.DotNet.Domain.Common;
using _4Create.DotNet.Domain.Employees;

namespace _4Create.DotNet.Domain.Companies;

public sealed class Company : Aggregate<Guid>
{
    public string Name { get; private set; }
    public IReadOnlySet<Guid> EmployeeIds => _employeeIds;
    private readonly HashSet<Guid> _employeeIds;
    
    public Company(Guid id, string name, IEnumerable<Guid> employeeIds) : base(id)
    {
        Name = name;
        _employeeIds = employeeIds.ToHashSet();
    }

    public static Company Create(string name, IEnumerable<Guid> employeeIds)
    {
        var company = new Company(Guid.NewGuid(), name, employeeIds);
        company.AddEvent(new CompanyCreatedEvent(company));

        return company;
    }

    public void AddEmployee(Guid employeeId)
    {
        if (_employeeIds.Contains(employeeId))
            throw new DomainException($"Employee {employeeId} already exists within the company");
        
        _employeeIds.Add(employeeId);
        AddEvent(new EmployeeAddedToCompanyEvent(employeeId, Id));
    }

    public void AddEmployees(IReadOnlyList<Guid> employeeIds)
    {
        foreach (var id in employeeIds)
        {
            AddEmployee(id);
        }
    }
}