using _4Create.DotNet.Application.Common;
using _4Create.DotNet.Domain.Common;
using Microsoft.OpenApi.Models;

namespace _4Create.DotNet.Modules;

internal static class ApplicationModule
{
    internal static void AddApplicationModule(this WebApplicationBuilder builder)
    {
        builder.AddMediatrModule();
        builder.AddPersistenceModule();
        builder.AddMapsterModule();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1"
                });
        });
        
        builder.Services.AddScoped<IEventBus, MediatorEventBus>();
    }
}