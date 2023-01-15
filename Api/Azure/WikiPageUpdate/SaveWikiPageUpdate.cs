
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
 
namespace WikiPageApp.AzureFunctions;

public partial class WikiPageUpdateFunctions
{  

    [OpenApiOperation(operationId: "SaveWikiPageUpdate", tags: new[] { "wiki-page-update" })]
    [OpenApiRequestBody("application/json", typeof(Model.WikiPageUpdate), Description = "JSON request body ")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Model.WikiPageUpdate), Description = "The OK response message containing a JSON result.")]        
    [Function("SaveWikiPageUpdate")] 
        public async Task<IActionResult> Save([HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = "WikiPageUpdate")]
        HttpRequestData req)
    {  
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var obj = JsonConvert.DeserializeObject<Model.WikiPageUpdate>(requestBody);
        
        var existing = await _context.WikiPageUpdate.FindAsync(obj.WikiPageUpdateID);

        if (existing == null) _context.WikiPageUpdate.Add(obj);
        else
        {
            _context.WikiPageUpdate.Remove(existing);
            _context.WikiPageUpdate.Add(obj);
        }

        await _context.SaveChangesAsync();
        return new OkObjectResult(obj);
    }
         
}