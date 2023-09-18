using System.Data;
using _4Create.DotNet.Persistence.Records;
using Dapper;

namespace _4Create.DotNet.Persistence.Repositories;

public sealed class CompaniesRepository : ICompaniesRepository
{
    // Used for brevity, in actual env we should use a single transaction per API call and rollback on exception 
    private readonly IDbConnection _connection;

    public CompaniesRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task InsertAsync(CompanyRecord company, CancellationToken ct)
    {
        await _connection.ExecuteAsync(new CommandDefinition(CompaniesQueries.InsertCompany, company, cancellationToken: ct));

        foreach (var employeeId in company.EmployeeIds)
        {
            await _connection.ExecuteAsync(new CommandDefinition(
                CompaniesQueries.InsertEmployee,
                new
                {
                    employeeId,
                    companyId = company.Id
                },
                cancellationToken: ct
            ));
        }
    }

    public async Task<IReadOnlyList<CompanyRecord>> GetCompaniesByIdsAsync(IReadOnlyList<Guid> companyIds, CancellationToken ct)
    {
        if (!companyIds.Any()) return Array.Empty<CompanyRecord>();
        
        var result = await _connection.QueryAsync<CompanyRecord>(
            new CommandDefinition(
                CompaniesQueries.GetCompanies, new
                {
                    companyIds
                },
                cancellationToken: ct
            ));

        return result.ToList();
    }

    public async Task<bool> DoesCompanyWithNameExist(string companyName, CancellationToken ct)
    {
        var result = await _connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            CompaniesQueries.DoesCompanyExist, 
            new
            {
                companyName
            },
            cancellationToken: ct));

        return result;
    }
}

internal static class CompaniesQueries
{
    internal const string InsertCompany = @"
INSERT INTO Company (id, name, created_at) 
VALUES (@id, @name, @createdAt);
";

    internal const string InsertEmployee = @"
INSERT INTO EmployeeCompany (employee_id, company_id)
VALUES (@employeeId, @companyId);
";

    internal const string GetCompanies = @"
SELECT
    c.id AS id,
    c.name AS name,
    ARRAY_AGG(ec.employee_id) AS employeeids,
    c.created_at AS createdat
FROM
    EmployeeCompany ec
        INNER JOIN
    Company c ON ec.company_id = c.id
WHERE c.id = ANY(@companyIds)
GROUP BY
    c.id, c.name, c.created_at
";

    internal const string DoesCompanyExist = @"
SELECT EXISTS(SELECT 1 FROM Company WHERE name = @companyName);
";
}