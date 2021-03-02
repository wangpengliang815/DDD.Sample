using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration
            , IHostingEnvironment env)
        {
            Configuration = configuration;
            environment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return DefaultLauncher.ConfigureServices(services, Configuration, environment);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            DefaultLauncher.BuildServices(app, env);
        }
    }
}
