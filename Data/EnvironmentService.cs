

    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Data;
public class EnvironmentService : IEnvironmentService
{
    // RESOURCE: https://csharpindepth.com/articles/singleton
    private static readonly Lazy<EnvironmentService> _service = new Lazy<EnvironmentService>(() => new EnvironmentService());

    public static EnvironmentService Instance { get { return _service.Value; } }

    private EnvironmentService()
    {
    }
    public string GetEnvironmentVariable(string variable)
    {
        var output = System.Environment.GetEnvironmentVariable(variable);
        return output;
    }

    public string GetCurrentEnvironment()
    {
        var output = GetEnvironmentVariable("environment");
        if (string.IsNullOrEmpty(output))
        {
            output = "Development";
        }
        return output;
    }
}