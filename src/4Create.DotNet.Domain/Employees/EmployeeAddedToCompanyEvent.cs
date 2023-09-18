using _4Create.DotNet.Domain.Common;

namespace _4Create.DotNet.Domain.Employees;

public sealed record EmployeeAddedToCompanyEvent(Guid EmployeeId, Guid CompanyId) : IDomainEvent;