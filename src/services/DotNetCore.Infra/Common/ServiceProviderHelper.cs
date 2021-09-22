namespace DotNetCore.Infra.Common
{
    using System;

    public static class ServiceProviderHelper
    {
        public static void Configure(IServiceProvider serviceProvider)
        {
            Current = serviceProvider;
        }

        public static IServiceProvider Current { get; private set; }
    }
}
