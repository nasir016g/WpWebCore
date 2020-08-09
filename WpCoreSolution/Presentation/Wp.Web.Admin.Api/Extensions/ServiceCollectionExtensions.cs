using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Wp.Web.Api.Admin.Extensions
{
    public static class ServiceCollectionExtensions
    {    
       
        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddCookie()
          .AddJwtBearer(jwtBearerOptions =>
          {
              jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
              {
                  ValidateActor = false,
                  ValidateAudience = false,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = configuration["Token:Issuer"],
                  ValidAudience = configuration["Token:Audience"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                                                     (configuration["Token:Key"]))
              };
          });
            return services;
        }

        
    }
}
