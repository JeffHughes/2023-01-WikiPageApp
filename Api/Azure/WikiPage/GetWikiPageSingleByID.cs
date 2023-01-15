                    using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models; 
using Microsoft.EntityFrameworkCore;

namespace WikiPageApp.AzureFunctions;
 
public partial class WikiPageFunctions
{   
    [OpenApiOperation(operationId: "GetWikiPageSingleByID", tags: new[] { "wiki-page" })]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "id of the WikiPage")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Model.WikiPage), Description = "The OK response message containing a JSON result.")]
    [Function("GetWikiPageSingleByID")]
    public async Task<IActionResult> GetWikiPageSingleByID(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WikiPage/{id:guid}")]
        HttpRequestData req,
    Guid id,
    ILogger log)
    {
       // id ??= new Guid( req.Query["id"] );

       // if (id == null) return NotFound();            
        var obj = await _context.WikiPage.FirstOrDefaultAsync(m => m.WikiPageID == id);
       // if (obj == null) return NotFound();            
        return new OkObjectResult(obj);
    }
    
    /* in testing, I THINK this doesn't work because we're using GUIDs!

    [OpenApiOperation(operationId: "GetWikiPageWithPartitionKey", tags: new[] { "wiki-page" })]
  [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "id of the Resource")]
  [OpenApiParameter(name: "pk", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "partition key of the Resource")]

  [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Model.WikiPage), Description = "The OK response message containing a JSON result.")]
  [FunctionName("GetWikiPageSingleByPartitionKey")]
  public async Task<IActionResult> GetWikiPageSingleByPartitionKey(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WikiPage/{id:guid}/{pk:guid}")]
      HttpRequest req,
      Guid id,
      Guid pk,
      ILogger log)
  {
      var cosmosClient = new CosmosClient("https://220412test.documents.azure.com:443/", "LIch86v1aAlsyj8kg22fncgwHCIfKkkj89jQP9T5hvIPR2BHftyB6RunaKhwvVKwVZyid3YsisyCGGLOSV2vMg==",
          new CosmosClientOptions() { ApplicationName = "WikiPageAppV1" });
      var database = cosmosClient.GetDatabase("WikiPageAppV1");
      var container = database.GetContainer("WikiPage");
      var obj = await container.ReadItemAsync<WikiPage>(id.ToString(), new PartitionKey(pk.ToString()));
      return new OkObjectResult(obj);
  }
  */
      
}