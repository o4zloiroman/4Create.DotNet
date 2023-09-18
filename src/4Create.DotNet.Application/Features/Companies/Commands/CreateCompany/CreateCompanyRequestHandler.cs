using _4Create.DotNet.Application.Features.Employees.Dtos;
using _4Create.DotNet.Domain.Common;
using _4Create.DotNet.Domain.Companies;
using _4Create.DotNet.Domain.Employees;
using _4Create.DotNet.Persistence.Records;
using _4Create.DotNet.Persistence.Repositories;
using MapsterMapper;
using MediatR;
using EmployeeTitle = _4Create.DotNet.Domain.Employees.EmployeeTitle;

namespace _4Create.DotNet.Application.Features.Companies.Commands.CreateCompany;

public sealed class CreateCompanyRequestHandler : IRequestHandler<CreateCompanyRequest>
{
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IEmployeesRepository _employeesRepository;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;

    public CreateCompanyRequestHandler(
        ICompaniesRepository companiesRepository, 
        IEventBus eventBus, 
        IEmployeesRepository employeesRepository, 
        IMapper mapper)
    {
        _companiesRepository = companiesRepository;
        _eventBus = eventBus;
        _employeesRepository = employeesRepository;
        _mapper = mapper;
    }

    public async Task Handle(CreateCompanyRequest request, CancellationToken ct)
    {
        var (companyName, employeeDtos) = request;

        if (!employeeDtos.Any())
            throw new DomainException("Company has to have at least one employee");
        
        await ValidateCompanyNameAsync(request, ct);

        var existingEmployeeIds = employeeDtos
            .Where(x => x.Id is not null)
            .Select(x => x.Id!.Value)
            .ToList();
        var existingEmployees = await _employeesRepository.GetEmployeesByIdsAsync(existingEmployeeIds, ct);
        var newEmployees = employeeDtos.Where(x => x.Id is null).ToList();
        ValidateEmployeesTitles(newEmployees, existingEmployees);
        
        var company = Company.Create(companyName, existingEmployeeIds);
        await _companiesRepository.InsertAsync(_mapper.Map<CompanyRecord>(company), ct);
        
        var employeesToCreate = employeeDtos.Where(x => x.Id == null);
        var createdEmployeeIds = await CreateEmployeesAsync(employeesToCreate, company.Id, ct);
        company.AddEmployees(createdEmployeeIds); 
        
        await _eventBus.PublishAllAsync(company.PopAllEvents(), ct);
    }

    private async Task ValidateCompanyNameAsync(CreateCompanyRequest request, CancellationToken ct)
    {
        var companyNameAlreadyExists = await _companiesRepository.DoesCompanyWithNameExist(request.CompanyName, ct);
        if (companyNameAlreadyExists)
            throw new DomainException("The company name is already taken");
    }
    
    // Not a fan of sacrificing readability for reusability in this caseso
    // If I were to, then I'd do it via a domain service, delegates, etc.
    // Domain purity would've been achieved by moving rep interfaces to a specific project
    private async Task<IReadOnlyList<Guid>> CreateEmployeesAsync(
        IEnumerable<EmployeeDto> employeeDtos,
        Guid companyId,
        CancellationToken ct)
    {
        var employeeIds = new List<Guid>();
        foreach (var employeeDto in employeeDtos)
        {
            await ValidateEmployeeEmailAsync(employeeDto.Email!, ct);
            
            var employee = Employee.Create(employeeDto.Email!, employeeDto.Title!.Value, companyId); 
            
            await _employeesRepository.InsertAsync(_mapper.Map<EmployeeRecord>(employee), ct);
            await _eventBus.PublishAllAsync(employee.PopAllEvents(), ct);
            
            employeeIds.Add(employee.Id);
        }

        return employeeIds;
    }
    
    private async Task ValidateEmployeeEmailAsync(string email, CancellationToken ct)
    {
        // todo email format validation in a form of a value object
        
        var emailAlreadyExists = await _employeesRepository.DoesEmployeeWithEmailExistAsync(email, ct);
        if (emailAlreadyExists)
            // For brevity we raise the exception immideatly without going through the rest 
            throw new DomainException($"Email {email} is already taken");
    }

    private void ValidateEmployeesTitles(
        IReadOnlyList<EmployeeDto> newEmployees, 
        IReadOnlyList<EmployeeRecord> existingEmployees)
    {
        var titles = newEmployees.Select(x => x.Title!.Value).ToList();
        
        titles.AddRange(existingEmployees.Select(x => (EmployeeTitle) x.Title));
        
        if (titles.GroupBy(x => x).Any(x => x.Count() > 1))
            throw new DomainException("The can't have more than one of each titles");
    }
}