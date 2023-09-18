using _4Create.DotNet.Application.Features.Employees.Commands.CreateEmployee;
using _4Create.DotNet.WebRequests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _4Create.DotNet.Controllers;

[Route("api/employees")]
[ApiController]
public sealed class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task Create([FromBody] CreateEmployeeWebRequest webRequest, CancellationToken ct)
    {
        var request = new CreateEmployeeRequest(webRequest.Email, webRequest.Title, webRequest.CompanyIds);
        await _mediator.Send(request, ct);
    }
}
