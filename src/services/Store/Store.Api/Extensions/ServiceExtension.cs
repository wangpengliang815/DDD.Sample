using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.Swagger;

namespace Store.Api.Extensions
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

                options.SwaggerDoc(version, new Info
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
