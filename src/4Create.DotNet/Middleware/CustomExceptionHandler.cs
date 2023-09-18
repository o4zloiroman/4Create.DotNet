using System.ComponentModel.DataAnnotations;
using System.Net;
using _4Create.DotNet.Domain.Common;

namespace _4Create.DotNet.Middleware;

internal sealed class CustomExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly bool _includeDetails; 
    
    public CustomExceptionHandler(RequestDelegate next, bool includeDetails = false) // for brevity
    {
        _next = next;
        _includeDetails = includeDetails;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = GetExceptionResponse(exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = GetStatusCode(exception);

        return context.Response
            .WriteAsJsonAsync(response);
    }

    private ExceptionResponse GetExceptionResponse(Exception exception)
    {
        var message = exception.Message;
        var details = _includeDetails ? exception.ToString() : null;

        return new ExceptionResponse(exception.GetType().Name, message, details);
    }

    private static int GetStatusCode(Exception exception) =>
        (int)(exception switch
        {
            ValidationException or DomainException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        });
}

internal sealed record ExceptionResponse(string Type, string Message, string? Details);