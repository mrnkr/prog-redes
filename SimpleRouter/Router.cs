using System;
using System.Linq;
using System.Reflection;

namespace SimpleRouter
{
    public class Router
    {
        public static void ListPossibleOperations()
        {
            Console.WriteLine("Operaciones posibles");
            Console.WriteLine("--------------------");
            Console.WriteLine("");

            var controllers = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(SimpleController).IsAssignableFrom(t));

            foreach (var ctrl in controllers)
            {
                var meta = ctrl
                    .GetMethods()
                    .Where(m => m.GetCustomAttribute<SimpleHandler>() != null)
                    .Select(m => m.GetCustomAttribute<SimpleHandler>());

                foreach (var decl in meta)
                {
                    Console.WriteLine($"{decl.OperationId} - {decl.Summary}");
                }
            }

            Console.WriteLine("exit - Salir de la aplicacion");
            Console.WriteLine("");
        }

        public static void RouteOperation(string code, object[] inject)
        {
            var called = false;

            var controllers = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(SimpleController).IsAssignableFrom(t));

            foreach (var ctrl in controllers)
            {
                var handler = ctrl
                    .GetMethods()
                    .Where(m => m.GetCustomAttribute<SimpleHandler>() != null)
                    .Where(m => m.GetCustomAttribute<SimpleHandler>().OperationId == code)
                    .FirstOrDefault();

                if (handler == null)
                {
                    continue;
                }

                called = CallHandler(ctrl, handler, inject);
                break;
            }

            if (!called)
            {
                Console.WriteLine("No se encontro operacion asociada a ese codigo... Intente nuevamente!");
            }
        }

        private static bool CallHandler(Type ctrl, MethodInfo handler, object[] inject)
        {
            var ctrlInstance = Activator.CreateInstance(ctrl, inject);
            handler.Invoke(ctrlInstance, new object[] { });
            return true;
        }
    }
}
