using _4Create.DotNet.Domain.Employees;

namespace _4Create.DotNet.Application.Features.Employees.Dtos;

public sealed record EmployeeDto(Guid? Id, string? Email, EmployeeTitle? Title);