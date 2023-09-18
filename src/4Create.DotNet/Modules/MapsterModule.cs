using _4Create.DotNet.Application.Common;
using Mapster;
using MapsterMapper;

namespace _4Create.DotNet.Modules;

internal static class MapsterModule
{
    internal static void AddMapsterModule(this WebApplicationBuilder builder)
    {
        var mappingConfig = new TypeAdapterConfig();
        mappingConfig.Default.MapToConstructor(true);
        mappingConfig.Scan(typeof(MappingConfig).Assembly);

        builder.Services.AddSingleton(mappingConfig);
        builder.Services.AddSingleton<IMapper, Mapper>();
    }
}