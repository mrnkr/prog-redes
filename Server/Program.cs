using Helpers;
using SimpleRouter;
using Subarashii.Core;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

            var ipAddr = PromptUserForIpAddress();
            var server = new Server(ipAddr, GetValueFromConfig<int>("port"));
            BeginInteractive(server);
            server.Run();
        }

        private static string PromptUserForIpAddress()
        {
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddresses = ipHostInfo.AddressList
                .Where(i => i.AddressFamily == AddressFamily.InterNetwork)
                .Select(i => i.ToString());

            if (ipAddresses.Count() == 0)
            {
                return "127.0.0.1";
            }

            if (ipAddresses.Count() == 1)
            {
                return ipAddresses.Single();
            }

            Console.WriteLine("Direcciones IP de este equipo");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("");
            ConsolePrompts.PrintListWithIndices(ipAddresses);
            Console.WriteLine("");

            var option = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Numero de la IP deseada",
                min: 1,
                max: ipAddresses.Count());

            return ipAddresses.ElementAt(option - 1);
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

        private static T GetValueFromConfig<T>(string key)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
