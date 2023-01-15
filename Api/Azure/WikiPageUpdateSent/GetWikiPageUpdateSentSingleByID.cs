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
 
public partial class WikiPageUpdateSentFunctions
{   
    [OpenApiOperation(operationId: "GetWikiPageUpdateSentSingleByID", tags: new[] { "wiki-page-update-sent" })]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "id of the WikiPageUpdateSent")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Model.WikiPageUpdateSent), Description = "The OK response message containing a JSON result.")]
    [Function("GetWikiPageUpdateSentSingleByID")]
    public async Task<IActionResult> GetWikiPageUpdateSentSingleByID(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WikiPageUpdateSent/{id:guid}")]
        HttpRequestData req,
    Guid id,
    ILogger log)
    {
       // id ??= new Guid( req.Query["id"] );

       // if (id == null) return NotFound();            
        var obj = await _context.WikiPageUpdateSent.FirstOrDefaultAsync(m => m.WikiPageUpdateSentID == id);
       // if (obj == null) return NotFound();            
        return new OkObjectResult(obj);
    }
    
    /* in testing, I THINK this doesn't work because we're using GUIDs!

    [OpenApiOperation(operationId: "GetWikiPageUpdateSentWithPartitionKey", tags: new[] { "wiki-page-update-sent" })]
  [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "id of the Resource")]
  [OpenApiParameter(name: "pk", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "partition key of the Resource")]

  [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Model.WikiPageUpdateSent), Description = "The OK response message containing a JSON result.")]
  [FunctionName("GetWikiPageUpdateSentSingleByPartitionKey")]
  public async Task<IActionResult> GetWikiPageUpdateSentSingleByPartitionKey(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WikiPageUpdateSent/{id:guid}/{pk:guid}")]
      HttpRequest req,
      Guid id,
      Guid pk,
      ILogger log)
  {
      var cosmosClient = new CosmosClient("https://220412test.documents.azure.com:443/", "LIch86v1aAlsyj8kg22fncgwHCIfKkkj89jQP9T5hvIPR2BHftyB6RunaKhwvVKwVZyid3YsisyCGGLOSV2vMg==",
          new CosmosClientOptions() { ApplicationName = "WikiPageAppV1" });
      var database = cosmosClient.GetDatabase("WikiPageAppV1");
      var container = database.GetContainer("WikiPageUpdateSent");
      var obj = await container.ReadItemAsync<WikiPageUpdateSent>(id.ToString(), new PartitionKey(pk.ToString()));
      return new OkObjectResult(obj);
  }
  */
      
}