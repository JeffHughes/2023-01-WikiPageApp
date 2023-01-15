 
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace WikiPageApp.AzureFunctions.Custom;

public class ApiHealthCheckFunction
{  
    [OpenApiOperation(operationId: "GetHealthCheck", tags: new[] { "status" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Model.Healthcheck), Description = "The OK response message containing a JSON result.")]
    [Function("GetHealthCheck")]
    public async Task<IActionResult> GetHealthCheck(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "status")]
    HttpRequestData req,
    ILogger log)
    {
        var result = new Model.Healthcheck()
        {
            ApplicationName = "WikiPageApp",
            IsSuccess = true,
            StatusCode = 200,
            StatusMessage = "OK",
            StatusDateTimeUtc = DateTime.UtcNow,
        };

        return new OkObjectResult(result);
    }
}