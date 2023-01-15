    
using WikiPageApp.Data;
using WikiPageApp.Model;

namespace WikiPageApp.AzureFunctions;
 
public partial class WikiPageUpdateFunctions
{  
    private readonly WikiPageUpdateDataContext _context;

     public WikiPageUpdateFunctions() : base()
      {
          var objName = nameof(WikiPageUpdate); 
          var (connectionString, EFProvider) = ConfigUtils.GetConfig(objName); 
          _context = new WikiPageUpdateDataContext(connectionString, EFProvider);
      } 
} 