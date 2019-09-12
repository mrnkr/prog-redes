using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subarashii.Core;

namespace SubarashiiDemo.Srv
{
    public class ValuesController : Controller
    {
        [Handler("23")]
        public string ExecuteOrder23(string message, string auth)
        {
            return "General Kenobi";
        }
    }
}
