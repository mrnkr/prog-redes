using SimpleRouter;
using Subarashii.Core;
using System;
using System.Diagnostics;
using System.Threading;

namespace Gestion.Srv
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Bienvenido a Gestion 3.0\nPresiona enter para iniciar el servidor...");
            Console.ReadLine();

            var server = new Server(8000);
            BeginInteractive(server);
            server.Run();
        }

        private static void BeginInteractive(Server server)
        {
            new Thread(() =>
            {
                Thread.Sleep(300);
                while (true)
                {
                    Console.Clear();
                    // Print menu here
                    Console.WriteLine("Ingrese codigo de operacion...");
                    var option = Console.ReadLine();

                    if (option == "exit")
                    {
                        Process.GetCurrentProcess().Kill();
                    }

                    Router.RouteOperation(option, new object[] { server });
                    Console.WriteLine("Presiona enter para continuar");
                    Console.ReadKey();
                }
            }).Start();
        }
    }
}
