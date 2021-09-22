namespace Ordering.Api.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;

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
                options.IgnoreObsoleteProperties();
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });
            return services;
        }
    }
}
