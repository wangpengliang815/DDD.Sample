using System;
using System.Linq;
using System.Reflection;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment environment;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration
            , IWebHostEnvironment env)
        {
            Configuration = configuration;
            environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            DefaultLauncher.ConfigureServices(services, Configuration, environment);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(LoadAssemblies(new string[] {
                "Ordering.Infrastructure"
            })).AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        private static Assembly[] LoadAssemblies(params string[] assemblyNames)
        {
            if (assemblyNames == null || !assemblyNames.Any())
            {
                return new Assembly[0];
            }
            else
            {
                Assembly[] assemblies = assemblyNames.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p =>
                {
                    return Assembly.Load(p);
                }).ToArray();
                return assemblies;
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DefaultLauncher.BuildServices(app, env);
        }
    }
}
