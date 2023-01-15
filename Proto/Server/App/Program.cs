





// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WikiPageApp.Proto.Server.App;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });

