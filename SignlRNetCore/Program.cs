using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SignlRNetCore.Hubs;

namespace SignlRNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
             .AddCommandLine(args)
             .Build();
            CreateWebHostBuilder(args, config).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration config) =>


            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .ConfigureLogging(factory => {
                    factory.AddConsole();
                })
                .UseKestrel(options =>
                {
                    ////This is kestrel doing HTTP with middleware
                    options.ListenLocalhost(5000);
                    //options.ListenLocalhost(5001, builder =>
                    //{
                    //    builder.UseHttps();
                    //});

                    ////This is kestrel doing TCP with connectionware
                    options.ListenLocalhost(8009, builder =>
                    {
                        builder.UseHub<ChatHub>();
                    });
                })
                .UseIISIntegration()
                .UseStartup<Startup>();
        
    }
}
