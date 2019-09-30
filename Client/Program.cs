using System;
using SimpleRouter;
using Subarashii.Core;

namespace Gestion.Cli
{
   
    class Program
    {

        static void PrintMenuOptions()
        {
            Console.WriteLine("Presione 1 para ver los estados de las materias");
            Console.WriteLine("Presione 2 para inscribirse a una nueva materia");
            Console.WriteLine("Presione 3 para desincribirse de una materia");
            Console.WriteLine("Presione 4 para subir un archivo a una materia");
            Console.WriteLine("Presione 5 para ver sus notas por materia");
            Console.WriteLine("Presione 6 para salir");
        }

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
                Context context = new Context(auth);
                var subscription = client.ListenToNotifications(Console.WriteLine);

                while (true)
                {
                    Console.Clear();
                    PrintMenuOptions();
                    Console.WriteLine("Ingresa el codigo de la operacion que quieras ejecutar");
                    var option = Console.ReadLine();

                    if (option == "6")
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
