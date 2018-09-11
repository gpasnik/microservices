using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GP.Microservices.Common.Authentication
{
    public static class Extensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidateAudience = false
                    };
                    options.RequireHttpsMetadata = false;
                });
        }

        public static Guid GetUserId(this HttpContext context)
        {
            if (context.User?.HasClaim(c => c.Type.Contains("nameidentifier")) != true)
                return Guid.Empty;

            var id = context.User.FindFirst(c => c.Type.Contains("nameidentifier")).Value;

            return Guid.Parse(id);
        }

        public static string GetUsername(this HttpContext context)
            => context?.User?.Identity?.Name;
    }
}