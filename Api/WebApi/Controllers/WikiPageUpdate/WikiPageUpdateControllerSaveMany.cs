
    
    
    
    
    
    
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
        //[Route("WikiPageUpdates")]
        [Route(WikiPageUpdateRoutes.SaveMany)]
        public async Task<IActionResult> SaveMany([FromBody] List<Model.WikiPageUpdate> objList)
        {
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
}