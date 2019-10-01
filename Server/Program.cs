using Helpers;
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
                    Router.ListPossibleOperations();

                    var option = ConsolePrompts.ReadUntilValid(
                        prompt: "Codigo de operacion",
                        pattern: "^[0-9]+|(exit)$",
                        errorMsg: "Favor de ingresar un numero o exit");

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
