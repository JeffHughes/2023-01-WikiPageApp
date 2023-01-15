





using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
 
namespace WikiPageApp.AzureFunctions;

public partial class WikiPageFunctions
{  
    [OpenApiOperation(operationId: "SaveWikiPages", tags: new[] { "wiki-page" })]
    [OpenApiRequestBody("application/json", typeof(List<Model.WikiPage>), Description = "JSON request body ")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Model.WikiPage>), Description = "The OK response message containing a JSON result.")]        
    [Function("SaveWikiPages")] 
        public async Task<IActionResult> SaveMany([HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = "WikiPages")]
        HttpRequestData req)
    {  
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var objList = JsonConvert.DeserializeObject<List<Model.WikiPage>>(requestBody);
        List<Model.WikiPage> savedList = new List<Model.WikiPage>();
        foreach (var obj in objList)
        {
            try
            {
                var existing = await _context.WikiPage.FindAsync(obj.WikiPageID);

                if (existing == null) _context.WikiPage.Add(obj);
                else
                {
                    _context.WikiPage.Remove(existing);
                    _context.WikiPage.Add(obj);
                }
                await _context.SaveChangesAsync();
                savedList.Add(obj);
            }
            catch (Exception ex)
            {
                continue;
            }
        }
        return new OkObjectResult(objList);
    }
         
}