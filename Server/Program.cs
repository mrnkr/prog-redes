using Subarashii.Core;

namespace SubarashiiDemo.Srv
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server(8000);
            server.Run();
        }
    }
}
