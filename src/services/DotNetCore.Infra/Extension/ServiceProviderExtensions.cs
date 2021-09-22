namespace DotNetCore.Infra.Extension
{
    using System;

    using DotNetCore.Infra.Common;

    using Microsoft.AspNetCore.Builder;

    public static class ServiceProviderExtensions
    {
        public static IApplicationBuilder UseStaticServiceProvider(this IApplicationBuilder app)
        {
            ServiceProviderHelper.Configure(app.ApplicationServices);
            return app;
        }

        public static IServiceProvider UseStaticServiceProvider(this IServiceProvider serviceProvider)
        {
            ServiceProviderHelper.Configure(serviceProvider);
            return serviceProvider;
        }
    }
}
