
    
    
    
    
    
    
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;
using WikiPageApp.ApiCore.Routes;

namespace WikiPageApp.AwsLambdas.Controllers.WikiPageUpdate
{
    public partial class WikiPageUpdateController
    {
        // GET: WikiPageUpdate
        [HttpGet]
        //[Route("WikiPageUpdate/{id}")]
        [Route(WikiPageUpdateRoutes.GetSingleByID)]
        public async Task<IActionResult> GetSingleByID(Guid id)
        {
            var obj = await _context.WikiPageUpdate.FirstOrDefaultAsync(m => m.WikiPageUpdateID == id);
            return new OkObjectResult(obj);
        }

    }
}