
    
    
    
    
    
    
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//      Generated: Sun, 15 Jan 2023 23:38:58 GMT
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable disable
using WikiPageApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Community.OData.Linq;
using System.Linq.Dynamic.Core;
using WikiPageApp.ApiCore.Routes;

namespace WikiPageApp.WebApi.Controllers.WikiPageUpdateSent
{
    public partial class WikiPageUpdateSentController
    {
        [HttpGet]      
        // GET: WikiPageUpdateSent
        //[Route("WikiPageUpdateSents")]
        [Route(WikiPageUpdateSentRoutes.GetMany)]
        [EnableQuery(AllowedFunctions = AllowedFunctions.All)]
        public async Task<IActionResult> GetMany()
        {
            //this.HttpContext.Request.QueryString.QueryCollection[]
            return new OkObjectResult(await _context.WikiPageUpdateSent.ToListAsync());
        }
        [HttpGet]
        // GET: WikiPageUpdateSent
        //[Route("GetWikiPageUpdateSentManyWithLinq")]
        [Route(WikiPageUpdateSentRoutes.GetManyWithLinq)]
        [EnableQuery(AllowedFunctions = AllowedFunctions.All)]
        public async Task<IActionResult> GetManyWithLinq([FromQuery] GetLinqQueryParams request)
        {
            var whereClause = !string.IsNullOrEmpty(request.where) ? request.where?.ToString() : string.Empty;
            var orderbyClause = !string.IsNullOrEmpty(request.orderby) ? request.where?.ToString() : string.Empty;
            var takeClause = !string.IsNullOrEmpty(request.take) ? request.where?.ToString() : string.Empty;
            var skipClause = !string.IsNullOrEmpty(request.skip) ? request.where?.ToString() : string.Empty;

            var linqWhere = whereClause.UseFullPropertyNames(typeof(Model.WikiPageUpdateSent));
            var linqOrderBy = orderbyClause.UseFullPropertyNames(typeof(Model.WikiPageUpdateSent));

            var Take = string.IsNullOrEmpty(takeClause) ? 1000 : int.Parse(takeClause);
            var Skip = string.IsNullOrEmpty(skipClause) ? 0 : int.Parse(skipClause);

            if (Take > 1000) Take = 1000;

            List<Model.WikiPageUpdateSent> results;
            if (string.IsNullOrEmpty(linqOrderBy) && string.IsNullOrEmpty(linqWhere))
            {
                results = await _context.WikiPageUpdateSent.Skip(Skip).Take(Take).ToDynamicListAsync<Model.WikiPageUpdateSent>();
                return new OkObjectResult(results);
            }

            if (string.IsNullOrEmpty(linqOrderBy))
            {
                results = await _context.WikiPageUpdateSent.Where(linqWhere).Skip(Skip).Take(Take).ToDynamicListAsync<Model.WikiPageUpdateSent>();
                return new OkObjectResult(results);
            }

            if (string.IsNullOrEmpty(linqWhere))
            {
                results = await _context.WikiPageUpdateSent.OrderBy(linqOrderBy).Skip(Skip).Take(Take).ToDynamicListAsync<Model.WikiPageUpdateSent>();
                return new OkObjectResult(results);
            }

            results = await _context.WikiPageUpdateSent.Where(linqWhere).OrderBy(linqOrderBy).Skip(Skip).Take(Take).ToDynamicListAsync<Model.WikiPageUpdateSent>();
            return new OkObjectResult(results);
         
        }

        [HttpGet]
        //[Route("GetWikiPageUpdateSentManyWithOData")]
        [Route(WikiPageUpdateSentRoutes.GetManyWithOData)]
        public async Task<IActionResult> GetManyWithOData([FromQuery] QueryParameters request)
        {
            var results = await ConfigUtils.GetResultsFromODataEFContextObject(request, _context.WikiPageUpdateSent.OData());            
            return new OkObjectResult(results);
        }
        private async Task<List<Model.WikiPageUpdateSent>> Where(Dictionary<string, string> dict)
        {
            var keysAndValues = dict.Select(keyValuePair => $"{keyValuePair.Key} == \"{keyValuePair.Value}\"").ToList();
            string where = string.Join(" && ", keysAndValues);

            return await _context.WikiPageUpdateSent
                .Where(where)
                .ToDynamicListAsync<Model.WikiPageUpdateSent>();
        }
    }
}