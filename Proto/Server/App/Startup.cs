





using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiPageApp.Proto.Server.Services;

namespace WikiPageApp.Proto.Server.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();           
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                RegisterGrpcServices(endpoints);
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
        private static void RegisterGrpcServices(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder endpoints)
        {
            var registrar = new ProtoServerAppServiceRegistrar(endpoints);
            RegisterProtoServerServiceCommand.Execute(registrar);
        }
    }
}
