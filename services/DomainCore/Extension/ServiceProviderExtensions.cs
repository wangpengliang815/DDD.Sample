﻿using System;

using DomainCore.Common;

using Microsoft.AspNetCore.Builder;

namespace DomainCore.Extension
{
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
