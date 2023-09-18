using _4Create.DotNet.Domain.Companies;
using _4Create.DotNet.Domain.Employees;
using _4Create.DotNet.Persistence.Records;
using Mapster;

namespace _4Create.DotNet.Application.Common;

public sealed class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Company, CompanyRecord>()
            .MapWith(x => new CompanyRecord
            {
                Id = x.Id,
                Name = x.Name,
                EmployeeIds = x.EmployeeIds,
                CreatedAt = DateTime.UtcNow
            });

        config.NewConfig<Employee, EmployeeRecord>()
            .MapWith(x => new EmployeeRecord
            {
                Id = x.Id,
                Email = x.Email,
                Title = (Persistence.Records.EmployeeTitle) x.Title,
                CompanyIds = x.CompanyIds,
                CreatedAt = DateTime.UtcNow
            });
    }
}