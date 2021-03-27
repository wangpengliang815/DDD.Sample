using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Swagger;

namespace Ordering.Api.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services
            , string title
            , string xmlPath
            , string version = "v1")
        {
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);

                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = title,
                    Version = $"{version}"
                });
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });
            return services;
        }
    }
}
