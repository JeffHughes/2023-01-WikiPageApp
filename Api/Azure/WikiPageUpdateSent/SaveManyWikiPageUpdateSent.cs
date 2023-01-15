





using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
 
namespace WikiPageApp.AzureFunctions;

public partial class WikiPageUpdateSentFunctions
{  
    [OpenApiOperation(operationId: "SaveWikiPageUpdateSents", tags: new[] { "wiki-page-update-sent" })]
    [OpenApiRequestBody("application/json", typeof(List<Model.WikiPageUpdateSent>), Description = "JSON request body ")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Model.WikiPageUpdateSent>), Description = "The OK response message containing a JSON result.")]        
    [Function("SaveWikiPageUpdateSents")] 
        public async Task<IActionResult> SaveMany([HttpTrigger(AuthorizationLevel.Anonymous, "post", "put", Route = "WikiPageUpdateSents")]
        HttpRequestData req)
    {  
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var objList = JsonConvert.DeserializeObject<List<Model.WikiPageUpdateSent>>(requestBody);
        List<Model.WikiPageUpdateSent> savedList = new List<Model.WikiPageUpdateSent>();
        foreach (var obj in objList)
        {
            try
            {
                var existing = await _context.WikiPageUpdateSent.FindAsync(obj.WikiPageUpdateSentID);

                if (existing == null) _context.WikiPageUpdateSent.Add(obj);
                else
                {
                    _context.WikiPageUpdateSent.Remove(existing);
                    _context.WikiPageUpdateSent.Add(obj);
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