using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace lessson1.Services
{
    public static class AuthenticationConfigService
    {
        public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.TokenValidationParameters = UserTokenService.GetTokenValidationParameters(configuration);
                });

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
                cfg.AddPolicy("User", policy => policy.RequireClaim("Role", "User", "Admin"));

                cfg.AddPolicy("UserOrAdmin", policy =>
                    policy.RequireAssertion(context =>
                    {
                        // זהו ה-claim שמכיל את ה-UserId
                        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var userRole = context.User.FindFirst("Role")?.Value;

                        // אם המשתמש הוא Admin אז הוא יכול לגשת לכל משתמש
                        if (userRole == "Admin") return true;

                        // אם אין לנו Claim של UserId, אז נחזיר false
                        if (userIdClaim == null) return false;

                        // קבלת ה-userId מהנתיב
                        var routeUserId = (context.Resource as HttpContext)?.Request.RouteValues["userId"]?.ToString();

                        // השוואה בין ה-userId של ה-claim וה-userId ב-Route
                        return routeUserId != null && routeUserId == userIdClaim;
                    }));
            });
        }
    }
}
