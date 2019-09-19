using Subarashii.Core;

namespace SubarashiiDemo.Srv
{
    class Program
    {
        public static Server Server;

        static void Main(string[] args)
        {
            Server = new Server(8000);
            Server.Run();
        }
    }
}
