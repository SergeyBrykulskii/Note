using Note.Domain.Result;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Note.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    public async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        _logger.Error(ex, ex.Message);

        var response = ex switch
        {
            UnauthorizedAccessException =>
            new BaseResult() { ErrorMessage = ex.Message, ErrorCode = (int)HttpStatusCode.Unauthorized },
            _ => new BaseResult() { ErrorMessage = "InternalServerError", ErrorCode = (int)HttpStatusCode.InternalServerError },

        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)response.ErrorCode;
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}
