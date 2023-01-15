
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
 
namespace WikiPageApp.AzureFunctions;

public partial class WikiPageUpdateSentFunctions
{  

    [OpenApiOperation(operationId: "SaveWikiPageUpdateSent", tags: new[] { "wiki-page-update-sent" })]
    [OpenApiRequestBody("application/json", typeof(Model.WikiPageUpdateSent), Description = "JSON request body ")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Model.WikiPageUpdateSent), Description = "The OK response message containing a JSON result.")]        
    [Function("SaveWikiPageUpdateSent")] 
        public async Task<IActionResult> Save([HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = "WikiPageUpdateSent")]
        HttpRequestData req)
    {  
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var obj = JsonConvert.DeserializeObject<Model.WikiPageUpdateSent>(requestBody);
        
        var existing = await _context.WikiPageUpdateSent.FindAsync(obj.WikiPageUpdateSentID);

        if (existing == null) _context.WikiPageUpdateSent.Add(obj);
        else
        {
            _context.WikiPageUpdateSent.Remove(existing);
            _context.WikiPageUpdateSent.Add(obj);
        }

        await _context.SaveChangesAsync();
        return new OkObjectResult(obj);
    }
         
}