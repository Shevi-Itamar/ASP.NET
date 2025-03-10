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
                        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var userRole = context.User.FindFirst("Role")?.Value;

                        if (userRole == "Admin") return true;

                        if (userIdClaim == null) return false;

                        var routeUserId = (context.Resource as HttpContext)?.Request.RouteValues["userId"]?.ToString();

                        return routeUserId != null && routeUserId == userIdClaim;
                    }));
            });
        }
    }
}
