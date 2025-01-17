using lessson1.Exceptions;

namespace lessson1.MiddleWares
{
    public class CatchMiddleWare
    {
        private readonly RequestDelegate _next;

        public CatchMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (ItemNotFoundException ex)
            {
                httpContext.Response.StatusCode = 404;
                httpContext.Response.Clear();
                await httpContext.Response.WriteAsync(ex.Message);
            }
        }
    }

    public static class MiddlewareExtensions
    {

        public static IApplicationBuilder UseCatchMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CatchMiddleWare>();
        }
    }
}
