





using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;
using Community.OData.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using WikiPageApp.Data;

namespace WikiPageApp.AzureFunctions;

public partial class WikiPageFunctions
{   
    // [EnableQuery(MaxTop = 100, AllowedQueryOptions = AllowedQueryOptions.All)]
    [OpenApiParameter(name: "Where", In = ParameterLocation.Query, Required = false, Summary = "",
        Description = "text to linq", Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter(name: "OrderBy", In = ParameterLocation.Query, Required = false, Summary = "",
        Description = "text to linq", Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter(name: "Skip", In = ParameterLocation.Query, Required = false, Summary = "",
        Description = "text to linq", Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter(name: "Take", In = ParameterLocation.Query, Required = false, Summary = "",
        Description = "text to linq", Type = typeof(string), Visibility = OpenApiVisibilityType.Important)] 
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Model.WikiPage>), Description = "The OK response message containing a JSON result.")]        
    [OpenApiOperation(operationId: "GetWikiPageWithLinq", tags: new[] { "wiki-page" })]
    [Function("GetWikiPageManyWithLinq")]
    public async Task<IActionResult> GetWikiPageManyWithLinq( 
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WikiPages")]
        HttpRequestData req )
    { 
        // TODO: clean this up to be common/shared functionality and possibly use 3rd party lib to auto-extract to class/struct
        var functionContext = req.FunctionContext;
        var bindingContext = functionContext.BindingContext;
        var bindingData = bindingContext.BindingData;
        var whereClause = (bindingData.ContainsKey("where")) ? bindingData["where"]?.ToString() : string.Empty;
        var orderbyClause = (bindingData.ContainsKey("orderby")) ? bindingData["orderby"]?.ToString() : string.Empty;
        var takeClause = (bindingData.ContainsKey("take")) ? bindingData["take"]?.ToString() : string.Empty;
        var skipClause = (bindingData.ContainsKey("skip")) ? bindingData["skip"]?.ToString() : string.Empty;

        var linqWhere = whereClause.UseFullPropertyNames(typeof(Model.WikiPage));
        var linqOrderBy = orderbyClause.UseFullPropertyNames(typeof(Model.WikiPage));

        var Take = string.IsNullOrEmpty(takeClause) ? 1000 : int.Parse(takeClause);
        var Skip = string.IsNullOrEmpty(skipClause) ? 0 : int.Parse(skipClause);

        if (Take > 1000) Take = 1000;

        List<Model.WikiPage> results;
        if (string.IsNullOrEmpty(linqOrderBy) && string.IsNullOrEmpty(linqWhere))
        {
            results = await _context.WikiPage.Skip(Skip).Take(Take).ToDynamicListAsync<Model.WikiPage>();
            return new OkObjectResult(results);
        }
 
        if (string.IsNullOrEmpty(linqOrderBy))
        {
            results = await _context.WikiPage.Where(linqWhere).Skip(Skip).Take(Take).ToDynamicListAsync<Model.WikiPage>();
            return new OkObjectResult(results);
        }

        if (string.IsNullOrEmpty(linqWhere))
        {
            results = await _context.WikiPage.OrderBy(linqOrderBy).Skip(Skip).Take(Take).ToDynamicListAsync<Model.WikiPage>();
            return new OkObjectResult(results);
        }
             
        results = await _context.WikiPage.Where(linqWhere).OrderBy(linqOrderBy).Skip(Skip).Take(Take).ToDynamicListAsync<Model.WikiPage>();
        return new OkObjectResult(results);
    }

    // [EnableQuery(MaxTop = 100, AllowedQueryOptions = AllowedQueryOptions.All)]
    //[OpenApiParameter(name: "expand", In = ParameterLocation.Query, Required = false, Summary = "", Description = "Use to add related query data.", Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter(name: "$filter",In = ParameterLocation.Query, Required = false, Summary = "", Description = "A function that must evaluate to true for a record to be returned.", Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter(name: "$orderby", In = ParameterLocation.Query, Required = false, Summary = "", Description = "Determines what values are used to order a collection of records.", Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
    // [OpenApiParameter(name: "$select", In = ParameterLocation.Query, Required = false, Summary = "", Description = "Specifies a subset of properties to return. Use a comma separated list.", Type = typeof(string), Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter(name: "$skip",In = ParameterLocation.Query, Required = false, Summary = "", Description = "The number of records to skip.", Type = typeof(int), Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter(name: "$top", In = ParameterLocation.Query, Required = false, Summary = "", Description = "The max number of records.", Type = typeof(int), Visibility = OpenApiVisibilityType.Important)]
    // todo: this needs fixin in template 
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Model.WikiPage>), Description = "The OK response message containing a JSON result.")]        
    [OpenApiOperation(operationId: "GetWikiPageWithOData", tags: new[] { "wiki-page" })]
    [Function("GetWikiPageManyWithOdata")]
    public async Task<IActionResult> GetWikiPageManyWithOData(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "WikiPages/OData")]
        HttpRequestData req )
    { 
        // TODO: clean this up to be common/shared functionality and possibly use 3rd party lib to auto-extract to class/struct
        var functionContext = req.FunctionContext;
        var bindingContext = functionContext.BindingContext;
        var bindingData = bindingContext.BindingData;
        var filterClause = (bindingData.ContainsKey("$filter")) ? bindingData["$filter"]?.ToString() : string.Empty;
        var orderbyClause = (bindingData.ContainsKey("$orderby")) ? bindingData["$orderby"]?.ToString() : string.Empty;
        var skipClause = (bindingData.ContainsKey("$skip")) ? bindingData["$skip"]?.ToString() : string.Empty;
        var topClause = (bindingData.ContainsKey("$top")) ? bindingData["$top"]?.ToString() : string.Empty;
        var queryParameters = new QueryParameters()
        {
            Filter = filterClause,
            OrderBy = orderbyClause,
            Skip = skipClause,
            Top = topClause
        };
        var results = await ConfigUtils.GetResultsFromODataEFContextObject(queryParameters, _context.WikiPage.OData());
        return new OkObjectResult(results);
    } 

}