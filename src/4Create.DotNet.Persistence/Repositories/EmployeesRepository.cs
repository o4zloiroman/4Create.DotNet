using System.Data;
using _4Create.DotNet.Persistence.Records;
using Dapper;

namespace _4Create.DotNet.Persistence.Repositories;

public sealed class EmployeesRepository : IEmployeesRepository
{
    // Used for brevity, in actual env we should use a single transaction per API call and rollback on exception
    private readonly IDbConnection _connection;

    public EmployeesRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task InsertAsync(EmployeeRecord employee, CancellationToken ct)
    {
        await _connection.ExecuteAsync(
            new CommandDefinition(
                EmployeesQueries.InsertEmployee,
                new
                {
                    id = employee.Id,
                    title = employee.Title,
                    email = employee.Email,
                    createdAt = employee.CreatedAt
                },
                cancellationToken: ct));

        foreach (var companyId in employee.CompanyIds)
        {
            await _connection.ExecuteAsync(new CommandDefinition(EmployeesQueries.InsertCompany, new
            {
                employeeId = employee.Id,
                companyId = companyId
            }, cancellationToken: ct));
        }
    }

    public async Task<IReadOnlyList<EmployeeRecord>> GetEmployeesByIdsAsync(IReadOnlyList<Guid> employeeIds, CancellationToken ct)
    {
        if (!employeeIds.Any()) return Array.Empty<EmployeeRecord>();
        
        var result = await _connection.QueryAsync<EmployeeRecord>(
            new CommandDefinition(
            EmployeesQueries.GetEmployees, 
            new
            {
                employeeIds
            },
            cancellationToken: ct));

        return result.ToList();
    }

    public async Task<bool> DoesEmployeeWithEmailExistAsync(string email, CancellationToken ct)
    {
        var result = await _connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
            EmployeesQueries.DoesEmployeeExist, new 
            { 
                email 
            },
            cancellationToken: ct));

        return result;
    }
}

internal static class EmployeesQueries
{
    internal const string InsertEmployee = @"
INSERT INTO Employee (id, title, email, created_at)
VALUES (@id, @title, @email, @createdAt);
";

    internal const string InsertCompany = @"
INSERT INTO EmployeeCompany (employee_id, company_id)
VALUES (@employeeId, @companyId);
";

    internal const string DoesEmployeeExist = @"
SELECT EXISTS(SELECT 1 FROM Employee WHERE email = @email);
";

    internal const string GetEmployees = @"
SELECT
    e.id AS Id,
    e.email AS Email,
    e.title AS Title,
    ARRAY_AGG(ec.company_id) AS CompanyIds,
    e.created_at AS CreatedAt
FROM
    Employee e
LEFT JOIN
    EmployeeCompany ec ON e.id = ec.employee_id
WHERE Id = ANY(@employeeIds)
GROUP BY
    e.id, e.email, e.title, e.created_at;
";
}