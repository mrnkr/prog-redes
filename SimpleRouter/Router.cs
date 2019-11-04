using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleRouter
{
    public static class Router
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

        public static async Task RouteOperation(string code, object[] inject)
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

                called = await CallHandler(ctrl, handler, inject);
                break;
            }

            if (!called)
            {
                Console.WriteLine("No se encontro operacion asociada a ese codigo... Intente nuevamente!");
            }
        }

        private static async Task<bool> CallHandler(Type ctrl, MethodInfo handler, object[] inject)
        {
            try
            {
                var ctrlInstance = Activator.CreateInstance(ctrl, inject);
                var ret = handler.Invoke(ctrlInstance, new object[] { });

                if (handler.ReturnType == typeof(Task))
                {
                    var t = (Task)ret;
                    await t;
                }

                return true;
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }
    }
}
