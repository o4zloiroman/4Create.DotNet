using _4Create.DotNet.Application.Features.Companies.Commands.CreateCompany;
using _4Create.DotNet.WebRequests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _4Create.DotNet.Controllers;

[Route("api/companies")]
[ApiController]
public sealed class CompaniesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompaniesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task Create([FromBody] CreateCompanyWebRequest webRequest, CancellationToken ct)
    {
        var request = new CreateCompanyRequest(webRequest.Name, webRequest.Employees);
        await _mediator.Send(request, ct);
    }
}