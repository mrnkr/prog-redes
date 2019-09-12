using System;
using Subarashii.Core;
using SubarashiiDemo.Model;

namespace SubarashiiDemo.Srv
{
    public class ValuesController : Controller
    {
        [Handler("23")]
        public string ExecuteOrder23(string message, string auth)
        {
            Console.WriteLine("Received order 23");
            Console.WriteLine(message);
            Console.WriteLine(auth);
            return "General Kenobi";
        }

        [Handler("66")]
        public string ExecuteOrder66(User user, string auth)
        {
            Console.WriteLine("Received order 23");
            Console.WriteLine("{0} - {1} {2}", user.Id, user.FirstName, user.LastName);
            Console.WriteLine(auth);
            return "General Kenobi";
        }
    }
}
