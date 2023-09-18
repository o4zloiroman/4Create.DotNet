using _4Create.DotNet.Domain.Common;

namespace _4Create.DotNet.Domain.Employees;

public sealed record EmployeeCreatedEvent(Employee Employee) : IDomainEvent;