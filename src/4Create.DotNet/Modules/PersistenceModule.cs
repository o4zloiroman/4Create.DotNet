using System.Data;
using _4Create.DotNet.Persistence.Repositories;
using Npgsql;

namespace _4Create.DotNet.Modules;

internal static class PersistenceModule
{
    internal static void AddPersistenceModule(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetValue<string>("Postgres:ConnectionString");
        builder.Services.AddTransient<IDbConnection>(_ => new NpgsqlConnection(connectionString));
        
        builder.Services.AddScoped<ISystemLogRepository, SystemLogRepository>();
        builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();
        builder.Services.AddScoped<ICompaniesRepository, CompaniesRepository>();
    }
}