using System;
using API.Extensions;

namespace API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var isDev = _env.IsEnvironment("Development") || _env.IsEnvironment("Local");
        var apiException = new ApiException(context.Response.StatusCode, exception.Message, isDev ? exception.StackTrace! : "Internal Server Error");
        return context.Response.WriteAsJsonAsync(apiException);
    }

}
