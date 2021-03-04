#define debugLogger
using System;
using System.Linq;
using System.Reflection;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using AutoMapper;

using DotnetCoreInfra.DataAccess;
using DotnetCoreInfra.DataAccessInterface;

using MediatR;

using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Ordering.Api.Extensions;
using Ordering.Application.CommandHandlers;
using Ordering.Infrastructure.Context;

namespace Ordering.Api
{
    public static class DefaultLauncher
    {
        private static ILoggerFactory LoggerFactory => new LoggerFactory()
                .AddDebug((categoryName, logLevel) => (logLevel == LogLevel.Information)
                && (categoryName == DbLoggerCategory.Database.Command.Name))
                .AddConsole((categoryName, logLevel) =>
                (logLevel == LogLevel.Information)
                && (categoryName == DbLoggerCategory.Database.Command.Name));

        public static IServiceProvider ConfigureServices(
              IServiceCollection services
            , IConfiguration Configuration
            , IHostingEnvironment environment)
        {
            // DbContext
            if (environment.IsDevelopment())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options
#if debugLogger
                    .UseLoggerFactory(LoggerFactory)
                    .EnableSensitiveDataLogging(true)
#endif
                    .UseSqlServer(Configuration
                        .GetSection("Database:ConnectString").Value);
                });
            }
            // OData
            services.AddOData();

            // OpenAPI            
            services.AddOpenApi("Ordering.Api", $"{AppDomain.CurrentDomain.BaseDirectory}{AppDomain.CurrentDomain.FriendlyName}.xml", "v1");

            // AutoMapper
            services.AddAutoMapper(GetAutoMapperProfiles());

            // HttpContext
            services.AddHttpContextAccessor();

            // 默认数据读写访问器
            services.AddScoped<IUnitOfWork, EfUnitOfWork<ApplicationDbContext>>();
            services.AddScoped<IDataAccessor, DataAccessor<ApplicationDbContext>>();

            // MediatR反射获取注入类型
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddMediatR(typeof(OrderCommandHandler).Assembly);

            // Workaround: https://github.com/OData/WebApi/issues/1177
            services.AddMvcCore(options =>
            {
                foreach (ODataOutputFormatter outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (ODataInputFormatter inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                // 下面这行代码不加的话单元测试找不到Controller,Workaround：https://stackoverflow.com/questions/57504788/testserver-returns-404-not-found
                .AddApplicationPart(typeof(Startup).Assembly);

            return RegisterAutofac(services);
        }

        /// <summary>Builds the services.</summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public static void BuildServices(IApplicationBuilder app
            , IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi("Ordering.Api", "/well-known/docs", version: "v1");

            app.UseMvc(routeBuilder =>
            {
                ODataConventionModelBuilder builder = new ODataConventionModelBuilder(app.ApplicationServices);
                routeBuilder.Select().OrderBy().Filter();
                routeBuilder.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel());
                // Workaround: https://github.com/OData/WebApi/issues/1175
                routeBuilder.EnableDependencyInjection();
            });
        }

        private static IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterAssemblyTypes(LoadAssemblies(new string[] {
                "Ordering.DomainService",
                "Ordering.Infrastructure"
            })).InstancePerLifetimeScope().AsImplementedInterfaces();

            AutofacServiceProvider serviceProvider =
                new AutofacServiceProvider(builder.Build());

            return serviceProvider;
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

        private static Assembly[] GetAutoMapperProfiles()
        {
            Assembly[] assemblies = new Assembly[] {
                // infra
                typeof(Ordering.Infrastructure.MapperProfiles.OrderingProfile).Assembly,
                //application
                //typeof(AppContextProfile).Assembly,
            };
            return assemblies;
        }
    }
}
