

    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Data
{
    public interface IEnvironmentService
    {
        public string GetEnvironmentVariable(string variable);

        public string GetCurrentEnvironment();
    }
}
