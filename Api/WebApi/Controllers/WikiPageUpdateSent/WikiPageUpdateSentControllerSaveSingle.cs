
    
    
    
    
    
    
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
namespace WikiPageApp.WebApi.Controllers.WikiPageUpdateSent
{
    public partial class WikiPageUpdateSentController
    {        
        [HttpPost]        
        //[Route("WikiPageUpdateSent")]
        [Route(WikiPageUpdateSentRoutes.SaveSingle)]
        public async Task<IActionResult>SaveSingle([FromBody] Model.WikiPageUpdateSent wikiPageUpdateSent)
        {
            var existing = await _context.WikiPageUpdateSent.FindAsync(wikiPageUpdateSent.WikiPageUpdateSentID);

            if (existing == null) _context.WikiPageUpdateSent.Add(wikiPageUpdateSent);
            else
            {
                _context.WikiPageUpdateSent.Remove(existing);
                _context.WikiPageUpdateSent.Add(wikiPageUpdateSent);
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult(wikiPageUpdateSent);
        }

    }
}