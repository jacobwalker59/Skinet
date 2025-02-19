using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Errors
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Skinet.API", Version = "v1"});
                var securitySchema = new OpenApiSecurityScheme 
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }

                };

                 c.AddSecurityDefinition("Bearer", securitySchema);
                 var securityRequirement = new OpenApiSecurityRequirement {{securitySchema, new []
                 {"Bearer"}}};
                    c.AddSecurityRequirement(securityRequirement);

            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => {c.SwaggerEndpoint("/swagger/v1/swagger.json", "SKinet API v1");});
            return app;
        }
    }
}