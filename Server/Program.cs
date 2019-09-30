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

        private static void PrintMenuOptions()
        {
            Console.WriteLine("Presione 1 para crear una nueva materia");
            Console.WriteLine("Presione 2 para crear un alumno nuevo");
            Console.WriteLine("Presione 3 para ver materias");
            Console.WriteLine("Presione 4 para ver alumnos");
            Console.WriteLine("Presione 5 para eliminar materia");
            Console.WriteLine("Presione 6 para eliminar alumno");
            Console.WriteLine("Presione 7 para asignar nota a alumno en una materia");
            Console.WriteLine("Presione 8 para salir");
        }

        private static void BeginInteractive(Server server)
        {
            new Thread(() =>
            {
                Thread.Sleep(300);
                while (true)
                {
                    Console.Clear();
                    PrintMenuOptions();
                    Console.WriteLine("Ingrese codigo de operacion...");
                    var option = Console.ReadLine();

                    if (option == "8")
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
