﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace VotingWeb
{
    using System;
    using System.Net.Http;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging.ApplicationInsights;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;
    using VotingWeb.Clients;

    public static class Program
    {


        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        
          public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                   var env = hostingContext.HostingEnvironment;
                   config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                         .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                             optional: false, reloadOnChange: true);
                   config.AddEnvironmentVariables();
               })
                .UseApplicationInsights()                
                .ConfigureLogging((hostingContext,logging) =>
                  {                    
                      logging.AddApplicationInsights((string)hostingContext
                             .Configuration         
                             .GetValue(typeof(string), "ApplicationInsights:InstrumentationKey"));
                      // Optional: Apply filters to configure LogLevel Trace or above is sent to
                      // ApplicationInsights for all categories.
                      logging.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Trace);
                      // Additional filtering For category starting in "Microsoft",
                      // only Warning or above will be sent to Application Insights.
                      logging.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Warning);
                  })
                .UseStartup<Startup>();

    }
}