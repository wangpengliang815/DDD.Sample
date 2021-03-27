#define debugLogger
using System;
using System.Linq;
using System.Reflection;

using Autofac;

using AutoMapper;

using DotNetCore.CAP.Messages;

using DotnetCoreInfra.DataAccess;
using DotnetCoreInfra.DataAccessInterface;
using DotnetCoreInfra.SeedWork;

using MediatR;

using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        public static void ConfigureServices(
              IServiceCollection services
            , IConfiguration Configuration
            , IWebHostEnvironment environment)
        {
            if (environment.EnvironmentName.Equals("Development"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(Configuration
                        .GetSection("Database:ConnectString").Value);
                });
            }

            services.AddControllers();

            services.AddCap(options =>
            {
                options.UseEntityFramework<ApplicationDbContext>();
                options.UseRabbitMQ("10.122.164.191");
                options.FailedRetryCount = 5;
                options.FailedThresholdCallback = failed =>
                {
                    var logger = failed.ServiceProvider.GetService<ILogger<Startup>>();
                    logger.LogError($@"A message of type {failed.MessageType} failed after executing {options.FailedRetryCount} several times, 
                        requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
                };
            });


            services.AddOpenApi("Ordering.Api", $"{AppDomain.CurrentDomain.BaseDirectory}{AppDomain.CurrentDomain.FriendlyName}.xml", "v1");

            services.AddAutoMapper(new Assembly[] {
                typeof(Infrastructure.MapperProfiles.OrderingProfile).Assembly,
                typeof(Application.MapperProfiles.OrderingProfile).Assembly,
            });

            services.AddHttpContextAccessor();

            services.AddScoped<IUnitOfWork, EfUnitOfWork<ApplicationDbContext>>();
            services.AddScoped<IDataAccessor, DataAccessor<ApplicationDbContext>>();

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

            services.AddControllersWithViews()
                // 下面这行代码不加的话单元测试找不到Controller,Workaround：https://stackoverflow.com/questions/57504788/testserver-returns-404-not-found
                .AddApplicationPart(typeof(Startup).Assembly);
        }

        public static void BuildServices(IApplicationBuilder app
            , IWebHostEnvironment env)
        {
            if (env.EnvironmentName.Equals("Development"))
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseOpenApi("Ordering.Api", "/well-known/docs", version: "v1");
        }
    }
}
