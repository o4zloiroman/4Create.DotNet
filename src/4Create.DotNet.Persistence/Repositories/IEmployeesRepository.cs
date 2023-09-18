using _4Create.DotNet.Persistence.Records;

namespace _4Create.DotNet.Persistence.Repositories;

public interface IEmployeesRepository
{
    Task InsertAsync(EmployeeRecord employee, CancellationToken ct);
    
    Task<IReadOnlyList<EmployeeRecord>> GetEmployeesByIdsAsync(IReadOnlyList<Guid> employeeIds, CancellationToken ct);
    Task<bool> DoesEmployeeWithEmailExistAsync(string email, CancellationToken ct);
}