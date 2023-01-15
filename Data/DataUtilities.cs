

    
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Community.OData.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WikiPageApp.Data;

public class CosmosConnectionSettings
{
    public string EndPoint { get; set; }

    public string AccountKey { get; set; }

    public string DatabaseName { get; set; }
}

public static class CosmosConnectionSettingsParser
{
    private static readonly string[] ParameterNames = new[] { "endpoint", "accountkey", "databasename" };
    private const string Delimiter = ";";
    private const string Separator = "=";

    public static CosmosConnectionSettings Parse(string connectionString)
    {
        var connectionStringFields = connectionString.Split(Delimiter);
        var parameterValues = new List<string>();
        foreach (var parameter in ParameterNames)
        {
            var item = connectionStringFields.FirstOrDefault(item => (item.ToLower().StartsWith(parameter)));
            var value = item.Substring(item.IndexOf(Separator) + 1);
            parameterValues.Add(value);
        }
        var output = new CosmosConnectionSettings()
        {
            EndPoint = parameterValues[0],
            AccountKey = parameterValues[1],
            DatabaseName = parameterValues[2]
        };
        return output;
    }
}

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseCosmos(this DbContextOptionsBuilder optionsBuilder, string connectionString, Action<CosmosDbContextOptionsBuilder> cosmosOptionsAction = null)
    {
        var cosmosConnectionSettings = CosmosConnectionSettingsParser.Parse(connectionString);
        optionsBuilder.UseCosmos(cosmosConnectionSettings.EndPoint, cosmosConnectionSettings.AccountKey, cosmosConnectionSettings.DatabaseName);
        return optionsBuilder;
    }
}

public class QueryParameters
{
    public string Select { get; set; }

    public string Filter { get; set; }

    public string OrderBy { get; set; }

    public string Top { get; set; }

    public string Skip { get; set; }
}

public static class ConfigUtils
{

 public static string GetRedisConfig(string appName)
    {
        var environment = "Production";
#if Development
        environment = "Development";
#elif Staging
           environment = "Staging";
#endif
        var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        dynamic config = JsonConvert.DeserializeObject(File.ReadAllText(binDirectory + "/appsettings.json"));
        var connectionStringName = $"{appName}{environment}RedisConnectionString";
        var connectionString = config[connectionStringName];
        return connectionString;
    }

    public static (string connectionString, string EFProvider) GetConfig(IConfiguration configuration, string objName)
    {
        var environment = "Production";
#if Development
       environment = "Development";
#elif Staging
       environment = "Staging";
#endif

        var envString = "Environments:" + environment + ":";

        var objectConfig = envString + objName + ":";
        var connectionStringName = configuration[objectConfig + "connectionString"];

        var connectionString = configuration["ConnectionStrings:" + connectionStringName];
        var EFProvider = configuration[objectConfig + "EFProvider"];

        return (connectionString, EFProvider);
    }


    public static void SetProvider(this DbContextOptionsBuilder optionsBuilder, string EFProvider, string connectionString)
    {
        switch (EFProvider.ToLower().Trim())
        {
            case "cosmos":
                optionsBuilder.UseCosmos(connectionString);
                break;

            case "sqlserver":
                optionsBuilder.UseSqlServer(connectionString);
                break;

            case "maria":
                optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.LatestSupportedServerVersion);
                break;

            default:
                break;
        }
    }

    public static (string connectionString, string EFProvider) GetConfig(string objName)
    {
        var environment = "Production";
#if Development
           environment = "Development";
#elif Staging
           environment = "Staging";
#endif
                
        var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        while (!File.Exists(binDirectory + "/appsettings.json") && binDirectory.Length > 5)
            binDirectory = Directory.GetParent(binDirectory!)?.ToString();

        dynamic config = JsonConvert.DeserializeObject(File.ReadAllText(binDirectory + "/appsettings.json"));
         
        dynamic obj = config["Environments"][environment][objName];
        var EFProvider = obj["EFProvider"];
        string connectionStringName = obj["connectionString"];
        dynamic connStrings = config["ConnectionStrings"];
        var connectionString = connStrings[connectionStringName];

        //var envString = "Environments:" + environment + ":";

        //var objectConfig = envString + objName + ":";
        //var connectionStringName = configuration[objectConfig + "connectionString"];

        //var connectionString = configuration["ConnectionStrings:" + connectionStringName];
        //var EFProvider = configuration[objectConfig + "EFProvider"];
 

        return (connectionString, EFProvider);
    }

   public static async Task<List<T>> GetResultsFromODataEFContextObject<T>(QueryParameters queryParameters, ODataQuery<T> odata)
    {
        var o = new Dictionary<string, string>
        {
            //{"select", queryParameters.Select },
            {"filter", queryParameters.Filter},
            {"orderby", queryParameters.OrderBy},
            {"top", queryParameters.Top},
            {"skip", queryParameters.Skip},
        };
         

        //if (!string.IsNullOrEmpty(o["select"])) odata = odata.SelectExpandAsQueryable(o["select"]);
        if (!string.IsNullOrEmpty(o["filter"])) odata = odata.Filter(o["filter"]);
        if (!string.IsNullOrEmpty(o["orderby"])) odata = odata.Filter(o["orderby"]);

        if (string.IsNullOrEmpty(o["top"])) o["top"] = "100";
        if (string.IsNullOrEmpty(o["skip"])) o["skip"] = "0";

        if (int.Parse(o["top"]) > 100) o["top"] = 100.ToString();
        if (int.Parse(o["skip"]) < 0) o["skip"] = 0.ToString();

        var results = await odata
            .TopSkip(o["top"], o["skip"])
            .ToOriginalQuery() // required to be able to use .ToListAsync() next.
            .ToListAsync();
        return results;
    }

   public static string UseFullPropertyNames(this string input, Type type)
   {
       if (string.IsNullOrEmpty(input)) return string.Empty;
 
       var props = Enumerable.ToList(type
                .GetProperties())
                .ToArray()
                .Select(x => x.Name)
                .Where(x => x.StartsWith(type.Name)) 
                .Select(x => x.Replace(type.Name, ""))  
                .ToDictionary(k => k.ToLower(), v => v);
  
       foreach (var component in input.Trim().ToLower().Split(' '))
           if (props.ContainsKey(component))
             input = Regex.Replace(input, component, type.Name + props[component], RegexOptions.IgnoreCase); 

       return input;
   }

}
