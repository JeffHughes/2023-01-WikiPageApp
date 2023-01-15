    
using WikiPageApp.Data;
using WikiPageApp.Model;

namespace WikiPageApp.AzureFunctions;
 
public partial class WikiPageFunctions
{  
    private readonly WikiPageDataContext _context;

     public WikiPageFunctions() : base()
      {
          var objName = nameof(WikiPage); 
          var (connectionString, EFProvider) = ConfigUtils.GetConfig(objName); 
          _context = new WikiPageDataContext(connectionString, EFProvider);
      } 
} 