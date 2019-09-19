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
                        Id = "222444",
                        FirstName = "Joselito",
                        LastName = "Vaca"
                    });
                    Console.WriteLine(client.Receive());

                    client.Authenticate("220159");

                    //client.SendFile("77", @"c:\Users\alvar\Pictures\tenor.gif");
                    //Console.WriteLine(client.Receive());

                    client.Send("88", "Gimme the file");
                    Console.WriteLine(client.ReceiveFile());

                    var subscription = client.ListenToNotifications(msg => Console.WriteLine(msg));

                    client.Send("23", "Hello there");
                    Console.WriteLine(client.Receive());

                    client.Send("66", new Student()
                    {
                        Id = "220159",
                        FirstName = "Alvaro",
                        LastName = "Nicoli"
                    });
                    Console.WriteLine(client.Receive());

                    client.Send("23", "Hello there");
                    Console.WriteLine(client.Receive());

                    subscription.Unsubscribe();

                    client.Send("23", "Hello there");
                    Console.WriteLine(client.Receive());

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
