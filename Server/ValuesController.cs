using System;
using Subarashii.Core;
using SubarashiiDemo.Model;

namespace SubarashiiDemo.Srv
{
    public class ValuesController : Controller
    {
        [Handler("23")]
        public void ExecuteOrder23(string message, string auth)
        {
            Console.WriteLine("Received order 23");
            Console.WriteLine(message);
            Console.WriteLine(auth);
            Text("General Kenobi");
        }

        [Handler("66")]
        public void ExecuteOrder66(User user, string auth)
        {
            Console.WriteLine("Received order 66");
            Console.WriteLine($"{user.Id} - {user.FirstName} {user.LastName}");
            Console.WriteLine(auth);
            Text("It\'s not the jedi way");
        }

        [Handler("77")]
        public void ExecuteOrder77(string file, string auth)
        {
            Console.WriteLine("Received order 77");
            Console.WriteLine(file);
            Text("Ok");
        }
    }
}
