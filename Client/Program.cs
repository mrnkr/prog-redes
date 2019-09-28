using System;
using SimpleRouter;
using Subarashii.Core;

namespace Gestion.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Bienvenido a Gestion 3.0\nPresiona enter para conectarte...");
            Console.ReadKey();

            var client = new Client("192.168.1.3", 8000);
            client.Connect(() =>
            {
                Console.Clear();
                Console.WriteLine("Por favor, ingresa tu numero de estudiante y presiona enter...");
                var auth = Console.ReadLine();
                client.Authenticate(auth);
                var subscription = client.ListenToNotifications(Console.WriteLine);

                while (true)
                {
                    Console.Clear();
                    // Print menu here
                    Console.WriteLine("Ingresa el codigo de la operacion que quieras ejecutar");
                    var option = Console.ReadLine();

                    if (option == "exit")
                    {
                        break;
                    }

                    Router.RouteOperation(option, new object[] { client });
                    Console.WriteLine("Presiona enter para continuar");
                    Console.ReadKey();
                }

                subscription.Unsubscribe();
                client.Dispose();
            });
        }
    }
}
