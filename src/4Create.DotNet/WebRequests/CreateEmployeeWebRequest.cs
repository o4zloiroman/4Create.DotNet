using _4Create.DotNet.Domain.Employees;

namespace _4Create.DotNet.WebRequests;

public sealed record CreateEmployeeWebRequest(string Email, EmployeeTitle Title, Guid[] CompanyIds);