using _4Create.DotNet.Application.Features.Companies.Commands.CreateCompany;

namespace _4Create.DotNet.Modules;

internal static class MediatrModule
{
    internal static void AddMediatrModule(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(CreateCompanyRequest).Assembly));
    }
}