
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace WikiPageApp.AzureFunctions;

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            // RESOURCE: https://github.com/Azure/azure-functions-host/issues/4464
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddJsonFile("appsettings.json");
            })

            // RESOURCE: https://devkimchi.com/2021/08/13/azure-functions-openapi-on-net5/
            // RESOURCE: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/general-availability-of-azure-functions-openapi-extension/ba-p/2931231
            .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
            .ConfigureOpenApi()

            .Build();

        host.Run();
    }
}
