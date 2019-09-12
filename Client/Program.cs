using System;
using Subarashii.Core;

namespace SubarashiiDemo.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client(8000);
            client.Connect();
            var response = client.Send("23", "Hello there");
            Console.WriteLine(response);
            client.Dispose();
        }
    }
}
