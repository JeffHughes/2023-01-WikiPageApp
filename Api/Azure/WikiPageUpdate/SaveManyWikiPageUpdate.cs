





using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
 
namespace WikiPageApp.AzureFunctions;

public partial class WikiPageUpdateFunctions
{  
    [OpenApiOperation(operationId: "SaveWikiPageUpdates", tags: new[] { "wiki-page-update" })]
    [OpenApiRequestBody("application/json", typeof(List<Model.WikiPageUpdate>), Description = "JSON request body ")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Model.WikiPageUpdate>), Description = "The OK response message containing a JSON result.")]        
    [Function("SaveWikiPageUpdates")] 
        public async Task<IActionResult> SaveMany([HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = "WikiPageUpdates")]
        HttpRequestData req)
    {  
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var objList = JsonConvert.DeserializeObject<List<Model.WikiPageUpdate>>(requestBody);
        List<Model.WikiPageUpdate> savedList = new List<Model.WikiPageUpdate>();
        foreach (var obj in objList)
        {
            try
            {
                var existing = await _context.WikiPageUpdate.FindAsync(obj.WikiPageUpdateID);

                if (existing == null) _context.WikiPageUpdate.Add(obj);
                else
                {
                    _context.WikiPageUpdate.Remove(existing);
                    _context.WikiPageUpdate.Add(obj);
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