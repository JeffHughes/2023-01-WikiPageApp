
    
    
    
    
    
    
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
using WikiPageApp.Data;
using WikiPageApp.ApiCore.Routes;
namespace WikiPageApp.WebApi.Controllers.WikiPageUpdate
{
    public partial class WikiPageUpdateController
    {        
        [HttpPost]        
        //[Route("WikiPageUpdate")]
        [Route(WikiPageUpdateRoutes.SaveSingle)]
        public async Task<IActionResult>SaveSingle([FromBody] Model.WikiPageUpdate wikiPageUpdate)
        {
            var existing = await _context.WikiPageUpdate.FindAsync(wikiPageUpdate.WikiPageUpdateID);

            if (existing == null) _context.WikiPageUpdate.Add(wikiPageUpdate);
            else
            {
                _context.WikiPageUpdate.Remove(existing);
                _context.WikiPageUpdate.Add(wikiPageUpdate);
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult(wikiPageUpdate);
        }

    }
}