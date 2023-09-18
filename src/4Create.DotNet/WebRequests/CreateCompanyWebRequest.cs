using _4Create.DotNet.Application.Features.Employees.Dtos;

namespace _4Create.DotNet.WebRequests;

public sealed record CreateCompanyWebRequest(string Name, EmployeeDto[] Employees);