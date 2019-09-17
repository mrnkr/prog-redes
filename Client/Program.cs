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
                    client.Send("66", new Student()
                    {
                        Id = "220159",
                        FirstName = "Alvaro",
                        LastName = "Nicoli"
                    });
                    Console.WriteLine(client.Recieve());

                    //client.SendFile("77", @"c:\Users\alvar\Pictures\tenor.gif");
                    //Console.WriteLine(client.Recieve());

                    client.Send("88", "Gimme the file");
                    Console.WriteLine(client.RecieveFile());

                    Console.ReadLine();
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
