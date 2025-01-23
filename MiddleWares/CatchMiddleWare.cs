using Microsoft.AspNetCore.Http.HttpResults;

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
            catch (Exception ex)
            {
  
                    httpContext.Response.Clear();
                    httpContext.Response.StatusCode = 400;
                    httpContext.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        Error = "Bad Request",
                        Message = ex.Message
                    };

                    await httpContext.Response.WriteAsJsonAsync(errorResponse);
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
