using System.Diagnostics;
using ASPNET.BackEnd.Common.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ASPNET.BackEnd.Common.Handlers;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger;
        _exceptionHandlers = new()
        {
            { typeof(Exception), HandleException },
        };
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (_exceptionHandlers.ContainsKey(exceptionType))
        {
            await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
            return true;
        }
        else
        {
            await HandleException(httpContext, exception);
            return true;
        }

    }

    private async Task HandleException(HttpContext httpContext, Exception ex)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        var statusCode = httpContext.Response.StatusCode != 200
            ? httpContext.Response.StatusCode
            : StatusCodes.Status500InternalServerError;

        var errorMessage = ex.Message;

        // Structured error log with stack trace, request method, route, and trace ID for issue tracing
        _logger.LogError(
            ex,
            "[HTTP {RequestMethod} {RequestPath}] Unhandled exception occurred. TraceId: {TraceId}. Error: {ErrorMessage}",
            httpContext.Request.Method,
            httpContext.Request.Path,
            traceId,
            errorMessage);

        var result = new ApiErrorResult
        {
            Code = statusCode,
            Message = $"Exception: {errorMessage}",
            TraceId = traceId,
            Error = new Error(ex.InnerException?.Message, ex.Source, ex.StackTrace, ex.GetType().Name)
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(result);
    }

}

