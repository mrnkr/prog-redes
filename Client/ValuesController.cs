using System;
using SimpleRouter;
using Subarashii.Core;

namespace Gestion.Cli
{
    public class ValuesController : SimpleController
    {
        private Client Client { get; }

        public ValuesController(Client c)
        {
            Client = c;
        }

        [SimpleHandler("66")]
        public void ExecuteOrder66()
        {
            Client.Send("23", "Hello there");
            var response = Client.Receive();
            Console.WriteLine(response);
        }
    }
}
