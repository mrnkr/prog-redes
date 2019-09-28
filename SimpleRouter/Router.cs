using System;
using System.Linq;
using System.Reflection;

namespace SimpleRouter
{
    public class Router
    {
        public static void RouteOperation(string code, object[] inject)
        {
            var controllers = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && typeof(SimpleController).IsAssignableFrom(t));

            foreach (var ctrl in controllers)
            {
                var handler = ctrl
                    .GetMethods()
                    .Where(m => m.GetCustomAttribute<SimpleHandler>().OperationId == code)
                    .FirstOrDefault();

                if (handler == null)
                {
                    continue;
                }

                CallHandler(ctrl, handler, inject);
                break;
            }
        }

        private static void CallHandler(Type ctrl, MethodInfo handler, object[] inject)
        {
            var ctrlInstance = Activator.CreateInstance(ctrl, inject);
            handler.Invoke(ctrlInstance, new object[] { });
        }
    }
}
