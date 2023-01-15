






using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace WikiPageApp.AzureFunctions;
 
public class WikiPageAppCosmosSetup
{   
  
    [OpenApiOperation(operationId: "WikiPageAppCosmosDBSetupDevelopment", tags: new[] { "cosmos-db-setup-development" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response message containing a JSON result.")]
    [Function("WikiPageAppCosmosDBSetupDevelopment")]
    public async Task WikiPageAppCosmosSetupFunctionDevelopment([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WikiPageAppCosmosSetupDevelopment")]
        HttpRequestData req)
    {     
        var url = "https://cosmos-psa-poc-dev-20220401.documents.azure.com:443/"; 
        var key = "556HGmZ1tlKsrc0cYv3wNIfEIbf8TplJZuzUwMUUkRQRVf9Wwr87ly4U3oX6DpYvuOe6UnqTjD5ktd4ND8o7Ww==";
        await CreateDB(url, key);
  } 

  [OpenApiOperation(operationId: "WikiPageAppCosmosDBSetupProduction", tags: new[] { "cosmos-db-setup-production" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response message containing a JSON result.")]
    [Function("WikiPageAppCosmosDBSetupProduction")]
    public async Task WikiPageAppCosmosSetupFunctionProduction([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WikiPageAppCosmosSetupProduction")]
        HttpRequestData req)
    {     
        var url = "https://cosmos-psa-poc-prod-20220401.documents.azure.com:443"; 
        var key = "K464vfOEyHy89jwmGpe9l9QIBld25JZJBx1Av8rklpqEm0pRAAzayNDh0j6syMfu9b6HKteFq3a2vYPmmAKmlw==";
        await CreateDB(url, key);
  } 

  private static async Task CreateDB(string url, string key)
  {
        var cosmosClient = new CosmosClient(url, key,
            new CosmosClientOptions() { ApplicationName = "WikiPageAppV1" });

        var databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync("WikiPageAppV1");
        var database = databaseResponse.Database;

     await database.CreateContainerIfNotExistsAsync("WikiPage", "/WikiPageID");
     await database.CreateContainerIfNotExistsAsync("WikiPageUpdate", "/WikiPageID");
     await database.CreateContainerIfNotExistsAsync("WikiPageUpdateSent", "/WikiPageUpdateID");
      

  }
}  