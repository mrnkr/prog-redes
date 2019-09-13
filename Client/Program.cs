using System;
using Subarashii.Core;
using SubarashiiDemo.Model;

namespace SubarashiiDemo.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            var client = new Client(8000);
            client.Connect(() =>
            {
                try
                {
                    var response = client.Send("66", new User()
                    {
                        Id = "220159",
                        FirstName = "Alvaro",
                        LastName = "Nicoli"
                    });
                    Console.WriteLine(response);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                client.Dispose();
                Console.ReadLine();
            });
        }
    }
}
