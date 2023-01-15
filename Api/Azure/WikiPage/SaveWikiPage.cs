
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
 
namespace WikiPageApp.AzureFunctions;

public partial class WikiPageFunctions
{  

    [OpenApiOperation(operationId: "SaveWikiPage", tags: new[] { "wiki-page" })]
    [OpenApiRequestBody("application/json", typeof(Model.WikiPage), Description = "JSON request body ")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Model.WikiPage), Description = "The OK response message containing a JSON result.")]        
    [Function("SaveWikiPage")] 
        public async Task<IActionResult> Save([HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = "WikiPage")]
        HttpRequestData req)
    {  
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var obj = JsonConvert.DeserializeObject<Model.WikiPage>(requestBody);
        
        var existing = await _context.WikiPage.FindAsync(obj.WikiPageID);

        if (existing == null) _context.WikiPage.Add(obj);
        else
        {
            _context.WikiPage.Remove(existing);
            _context.WikiPage.Add(obj);
        }

        await _context.SaveChangesAsync();
        return new OkObjectResult(obj);
    }
         
}