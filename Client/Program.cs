using Gestion.Common;
using SimpleRouter;
using Subarashii.Core;
using Subarashii.Core.Exceptions;
using System;
using System.Configuration;
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

            var client = new Client(GetValueFromConfig<string>("ip"), GetValueFromConfig<int>("port"));
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

                        try
                        {
                            Router.RouteOperation(option, new object[] { client });
                        }
                        catch (OperationFailedException)
                        {
                            Console.WriteLine("Ha ocurrido un error, la operacion se ha cancelado");
                        }

                        Console.WriteLine("Presiona enter para continuar");
                        Console.ReadKey();
                    }

                    subscription.Unsubscribe();
                }
                catch (InvalidAuthException)
                {
                    Console.WriteLine("No pareces estar registrado, contacta a un admin!");
                    Console.WriteLine("Adios!");
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
            try
            {
                var userId = ConsolePrompts.ReadUntilValid(
                    prompt: "Numero de estudiante",
                    pattern: "^[0-9]{6}$",
                    errorMsg: "Por favor, ingrese un numero de seis cifras. Complete con ceros a la izquierda de ser necesario.");

                c.Send("01", userId);
                var firstName = c.Receive();

                Console.WriteLine($"Hola {firstName}!");
                Thread.Sleep(1000);

                c.Authenticate(userId);
            }
            catch (OperationFailedException)
            {
                throw new InvalidAuthException();
            }
        }

        private static T GetValueFromConfig<T>(string key)
        {
            var value = ConfigurationManager.AppSettings.Get(key);
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
