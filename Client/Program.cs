using Gestion.Cli.Exceptions;
using Gestion.Model;
using Helpers;
using SimpleRouter;
using Subarashii.Core;
using System;
using System.Threading;

namespace Gestion.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Bienvenido a Gestion 3.0\nPresiona enter para conectarte...");
            Console.ReadKey();

            var client = new Client("192.168.1.12", 8000);
            client.Connect(() =>
            {
                try
                {
                    AuthenticateAgainstServer(client);
                    var subscription = client.ListenToNotifications(Console.WriteLine);

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
                            break;
                        }

                        Router.RouteOperation(option, new object[] { client });
                        Console.WriteLine("Presiona enter para continuar");
                        Console.ReadKey();
                    }

                    subscription.Unsubscribe();
                }
                catch (InvalidAuthException)
                {
                    Console.WriteLine("You don't seem to be registered, please contact the admin!");
                    Console.WriteLine("Bye!");
                    Thread.Sleep(2000);
                }
                finally
                {
                    client.Dispose();
                }
            });
        }

        private static void AuthenticateAgainstServer(Client c)
        {
            var userId = ConsolePrompts.ReadUntilValid(
                prompt: "Numero de estudiante",
                pattern: "^[0-9]{6}$",
                errorMsg: "Por favor, ingrese un numero de seis cifras. Complete con ceros a la izquierda de ser necesario.");

            c.Send("01", userId);
            var response = c.Receive();

            if (response != "HELO")
            {
                throw new InvalidAuthException();
            }

            c.Send("02", userId);
            var firstName = c.Receive();

            Console.WriteLine($"Hello {firstName}!");
            Thread.Sleep(1000);

            c.Authenticate(userId);
        }
    }
}
