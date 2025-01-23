using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;


namespace lessson1.MiddleWares
{
    public class LogFileMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly string _logFilePath;

        public LogFileMiddleWare(RequestDelegate next)
        {
            _next = next;
            _logFilePath = "logs.txt";
        }

        public async Task Invoke(HttpContext context)
        {
            var requestLog = $"[{DateTime.Now}] Request: {context.Request.Method} {context.Request.Path}\n";

            try
            {
                await File.AppendAllTextAsync(_logFilePath, requestLog);

                await _next(context);
            }
            catch (Exception ex)
            {
                var errorLog = $"[{DateTime.Now}] Error: {ex.Message}\n{ex.StackTrace}\n";

                await File.AppendAllTextAsync(_logFilePath, errorLog);

                throw;
            }
            finally
            {
                var responseLog = $"[{DateTime.Now}] Response: {context.Response.StatusCode}\n\n";
                await File.AppendAllTextAsync(_logFilePath, responseLog);
            }
        }
    }
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogFileMiddleWare>();
        }
    }
}
