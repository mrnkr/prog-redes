using Gestion.Common;
using SimpleRouter;
using Subarashii.Core;
using System;
using System.Diagnostics;
using System.Threading;

namespace Gestion.Srv
{
    public class CommandLineInterface
    {
        public static void BeginInteractive(Server server)
        {
            new Thread(async () =>
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

                    await Router.RouteOperation(option, new object[] { server });
                    Console.WriteLine("Presiona enter para continuar");
                    Console.ReadKey();
                }
            }).Start();
        }
    }
}
