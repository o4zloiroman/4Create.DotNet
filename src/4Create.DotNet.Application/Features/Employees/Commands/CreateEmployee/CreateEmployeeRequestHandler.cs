using _4Create.DotNet.Domain.Common;
using _4Create.DotNet.Domain.Employees;
using _4Create.DotNet.Persistence.Records;
using _4Create.DotNet.Persistence.Repositories;
using MapsterMapper;
using MediatR;
using EmployeeTitle = _4Create.DotNet.Domain.Employees.EmployeeTitle;

namespace _4Create.DotNet.Application.Features.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeRequestHandler : IRequestHandler<CreateEmployeeRequest>
{
    private readonly IEmployeesRepository _employeesRepository;
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;

    public CreateEmployeeRequestHandler(
        IEmployeesRepository employeesRepository, 
        ICompaniesRepository companiesRepository, 
        IEventBus eventBus, 
        IMapper mapper)
    {
        _employeesRepository = employeesRepository;
        _companiesRepository = companiesRepository;
        _eventBus = eventBus;
        _mapper = mapper;
    }

    public async Task Handle(CreateEmployeeRequest request, CancellationToken ct)
    {
        var (email, title, companyIds) = request;
        
        await ValidateEmail(email, ct);
        await ValidateTitle(title, companyIds, ct);

        var employee = Employee.Create(email, title, companyIds);

        await _employeesRepository.InsertAsync(_mapper.Map<EmployeeRecord>(employee), ct);
        await _eventBus.PublishAllAsync(employee.PopAllEvents(), ct);
    }

    private async Task ValidateEmail(string email, CancellationToken ct)
    {
        // todo email format validation in a form of a value object
        
        var emailAlreadyExists = await _employeesRepository.DoesEmployeeWithEmailExistAsync(email, ct);
        if (emailAlreadyExists)
            throw new DomainException($"The email {email} is already taken");
    }

    private async Task ValidateTitle(EmployeeTitle title, IReadOnlyList<Guid> companyIds, CancellationToken ct)
    {
        if (title is EmployeeTitle.Unknown)
            throw new DomainException("Wrong title value");
        
        var companiesToAddTo = await _companiesRepository.GetCompaniesByIdsAsync(companyIds, ct);
        foreach (var company in companiesToAddTo)
        {
            var employees = await _employeesRepository.GetEmployeesByIdsAsync(company.EmployeeIds.ToList(), ct);
            if (employees.Any(x => (EmployeeTitle) x.Title == title))
                // For brevity we raise the exception immideatly without going through the rest
                throw new DomainException($"Company {company.Name} already has an employee with the title of {title}");
        }
    }
}