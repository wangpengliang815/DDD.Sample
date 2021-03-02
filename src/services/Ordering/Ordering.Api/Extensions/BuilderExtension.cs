using Microsoft.AspNetCore.Builder;

namespace Ordering.Api.Extensions
{
    public static class BuilderExtension
    {
        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app
            , string title
            , string pattern = "/well-known/docs"
            , string version = "v1")
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = pattern.TrimStart('/') + "/{documentName}/swagger.json";
            });

            app.UseReDoc(options =>
            {
                options.DocumentTitle = title;
                options.RoutePrefix = pattern.TrimStart('/');
                options.SpecUrl = $"{version}/swagger.json";
            });

            return app;
        }
    }
}
