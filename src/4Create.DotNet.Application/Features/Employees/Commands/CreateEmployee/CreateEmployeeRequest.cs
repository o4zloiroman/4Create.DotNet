using _4Create.DotNet.Domain.Employees;
using MediatR;

namespace _4Create.DotNet.Application.Features.Employees.Commands.CreateEmployee;

public sealed record CreateEmployeeRequest(
    string Email,
    EmployeeTitle Title,
    IReadOnlyList<Guid> CompanyIds
) : IRequest;