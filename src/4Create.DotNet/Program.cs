using _4Create.DotNet.Middleware;
using _4Create.DotNet.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationModule();

var app = builder.Build();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<CustomExceptionHandler>();

app.Run();