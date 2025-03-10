using System.Diagnostics;
using Serilog;

namespace lessson1.Middlewares;

public class AuditLogMiddleware
{
    private readonly RequestDelegate next;

    public AuditLogMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext c)
    {
        var sw = new Stopwatch();
        sw.Start();

        Log.Information($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
                        + $" User: {c.User?.FindFirst("userName")?.Value ?? "unknown"}");

        await next.Invoke(c);
    }
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseAuditLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuditLogMiddleware>();
    }
}
