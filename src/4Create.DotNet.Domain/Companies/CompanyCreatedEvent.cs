using _4Create.DotNet.Domain.Common;

namespace _4Create.DotNet.Domain.Companies;

public sealed record CompanyCreatedEvent(Company Company) : IDomainEvent;