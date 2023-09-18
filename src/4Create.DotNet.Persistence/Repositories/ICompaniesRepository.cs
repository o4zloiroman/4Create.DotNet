using _4Create.DotNet.Persistence.Records;

namespace _4Create.DotNet.Persistence.Repositories;

public interface ICompaniesRepository
{
    Task InsertAsync(CompanyRecord company, CancellationToken ct);
    
    Task<IReadOnlyList<CompanyRecord>> GetCompaniesByIdsAsync(IReadOnlyList<Guid> companyIds, CancellationToken ct);
    Task<bool> DoesCompanyWithNameExist(string companyName, CancellationToken ct);
}