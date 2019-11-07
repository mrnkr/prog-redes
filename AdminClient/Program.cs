using Gestion.Admin.Cli.ViewModels;
using Gestion.Common;
using Gestion.Common.Exceptions;
using SimpleRouter;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Gestion.Admin.Cli
{
    class Program
    {
        private static HttpClient AdminClient { get; } = new HttpClient { BaseAddress = new Uri(Config.GetValue<string>("admin_api")) };
        private static HttpClient LogsClient { get; } = new HttpClient { BaseAddress = new Uri(Config.GetValue<string>("logs_api")) };

        static async Task Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Bienvenido al admin de Gestion 3.0\nPresiona enter para conectarte...");
            Console.ReadKey();

            try
            {
                await Authenticate();

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
                        await Router.RouteOperation(option, new object[] { AdminClient, LogsClient });
                    }
                    catch (OperationFailedException)
                    {
                        Console.WriteLine("Ha ocurrido un error, la operacion se ha cancelado");
                    }

                    Console.WriteLine("Presiona enter para continuar");
                    Console.ReadKey();
                }
            }
            catch
            {
                Console.WriteLine("No lo hemos podido identificar, vayase!");
                Thread.Sleep(2000);
            }
        }

        private static async Task Authenticate()
        {
            ConsolePrompts.PrintEmptyLine();

            var email = ConsolePrompts.ReadUntilValid(
                prompt: "Email",
                pattern: Constants.EMAIL_REGEX,
                errorMsg: "Ese email no parece valido... Por favor, vuelve a intentarlo");

            var password = ConsolePrompts.ReadUntilValid(
                prompt: "Contrasena",
                pattern: ".+",
                errorMsg: "Tu contrasena no puede estar vacia... Intenta nuevamente");

            Console.WriteLine("Cargando...");
            var response = await AdminClient.PostAndExpectObjectAsync<TokenResponseViewModel>("/api/login", new
            {
                username = email,
                password
            });

            AdminClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {response.token}");
            LogsClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {response.token}");
        }
    }
}
