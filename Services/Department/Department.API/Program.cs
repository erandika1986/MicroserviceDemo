
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Microsoft.Extensions.Options;
using Department.API.Data;

namespace Department.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
            .MigrateDbContext<DepartmentContext>((context, services) =>
            {
                var env = services.GetService<IHostingEnvironment>();
                var settings = services.GetService<IOptions<DepartmentSettings>>();
                var logger = services.GetService<ILogger<DepartmentContextSeed>>();

                new DepartmentContextSeed()
                .SeedAsync(context, env, settings, logger)
                .Wait();

            })
            .Run();
        }


        public static IWebHost BuildWebHost(string[] args) =>
             Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            //.UseIISIntegration()
            .UseStartup<Startup>()
            .UseWebRoot("Pics")
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                var builtConfig = config.Build();
                var configurationBuilder = new ConfigurationBuilder();

                configurationBuilder.AddEnvironmentVariables();
                config.AddConfiguration(configurationBuilder.Build());
            })
            .ConfigureLogging((hostingContext, builder) =>
            {
                builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                builder.AddConsole();
                builder.AddDebug();
            })
            .Build();
    }
}
