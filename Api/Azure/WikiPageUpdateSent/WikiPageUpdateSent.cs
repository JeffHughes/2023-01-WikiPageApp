    
using WikiPageApp.Data;
using WikiPageApp.Model;

namespace WikiPageApp.AzureFunctions;
 
public partial class WikiPageUpdateSentFunctions
{  
    private readonly WikiPageUpdateSentDataContext _context;

     public WikiPageUpdateSentFunctions() : base()
      {
          var objName = nameof(WikiPageUpdateSent); 
          var (connectionString, EFProvider) = ConfigUtils.GetConfig(objName); 
          _context = new WikiPageUpdateSentDataContext(connectionString, EFProvider);
      } 
} 