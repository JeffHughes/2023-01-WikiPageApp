
    
    
    
    
    
    
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
using Microsoft.Extensions.Configuration;
namespace WikiPageApp.AwsLambdas.Controllers.WikiPageUpdateSent
{

    [ApiController]
    //[RoutePrefix("api/WikiPageUpdateSents")]
    public partial class WikiPageUpdateSentController : Controller
    {
        private readonly WikiPageUpdateSentDataContext _context;

        public WikiPageUpdateSentController(IConfiguration configuration) : base()
        {
            var objName = nameof(WikiPageUpdateSent);
            var (connectionString, EFProvider) = ConfigUtils.GetConfig(objName);
            _context = new WikiPageUpdateSentDataContext(connectionString, EFProvider);
        }
       
    }

}
