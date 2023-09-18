using _4Create.DotNet.Application.Features.Employees.Dtos;
using MediatR;

namespace _4Create.DotNet.Application.Features.Companies.Commands.CreateCompany;

public sealed record CreateCompanyRequest(
    string CompanyName, 
    IReadOnlyList<EmployeeDto> Employees
) : IRequest;