
    
    
    
    
    
    
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
using WikiPageApp.Data;
using System.Web.Http;

namespace WikiPageApp.WebApi.Controllers.WikiPage
{

    [ApiController]
    //[RoutePrefix("api/WikiPages")]
    public partial class WikiPageController : Controller
    {
        private readonly WikiPageDataContext _context;

        public WikiPageController(IConfiguration configuration) : base()
        {
            var objName = nameof(WikiPage);
            var (connectionString, EFProvider) = ConfigUtils.GetConfig(objName);
            _context = new WikiPageDataContext(connectionString, EFProvider);
        }
       
    }

}