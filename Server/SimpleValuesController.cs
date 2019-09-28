using SimpleRouter;
using Subarashii.Core;
using System;

namespace Gestion.Srv
{
    public class SimpleValuesController : SimpleController
    {
        private Server Srv { get; }

        public SimpleValuesController(Server srv)
        {
            Srv = srv;
        }

        [SimpleHandler("66")]
        public void ExecuteOrder66()
        {
            Console.WriteLine("Doing admin things");
        }
    }
}
