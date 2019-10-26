using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gestion.Common
{
    public class ConsolePrompts
    {
        public static string ReadUntilValid(string prompt, string pattern, string errorMsg)
        {
            var regex = new Regex(pattern);

            Console.Write($"{prompt} >> ");
            var ret = Console.ReadLine();

            if (regex.IsMatch(ret))
            {
                return ret;
            }
            else
            {
                Console.WriteLine(errorMsg);
                return ReadUntilValid(prompt, pattern, errorMsg);
            }
        }

        public static int ReadNumberUntilValid(string prompt, int min, int max)
        {
            var input = ReadUntilValid(prompt, "^[0-9]+$", "Favor de ingresar un numero");
            var ret = int.Parse(input);

            if (ret >= min && ret <= max)
            {
                return ret;
            }
            else
            {
                Console.WriteLine($"Favor de ingresar un numero entre {min} y {max} inclusive");
                return ReadNumberUntilValid(prompt, min, max);
            }
        }

        public static void PrintListWithIndices(IEnumerable<string> es)
        {
            for (var i = 0; i < es.Count(); i++)
            {
                Console.WriteLine($"{i + 1} - {es.ElementAt(i)}");
            }
        }

        public static string ChooseFile(string currentDirectory = @"C:\")
        {
            var entries = Directory.EnumerateFileSystemEntries(currentDirectory);

            Console.Clear();
            Console.WriteLine($"Directorio activo: {currentDirectory}");
            Console.WriteLine("");

            if (currentDirectory != @"C:\")
            {
                Console.WriteLine("0 - Ir atras");
            }

            PrintListWithIndices(entries);
            Console.WriteLine("");

            var option = ReadNumberUntilValid(
                prompt: "Numero de opcion deseada",
                min: currentDirectory == @"C:\" ? 1 : 0,
                entries.Count());

            if (option == 0)
            {
                var nextDirectory = currentDirectory.Substring(0, currentDirectory.LastIndexOf('\\'));
                return ChooseFile(nextDirectory == @"C:" ? @"C:\" : nextDirectory);
            }

            var attr = File.GetAttributes(entries.ElementAt(option - 1));
            var isDirectory = (attr & FileAttributes.Directory) == FileAttributes.Directory;

            if (isDirectory)
            {
                return ChooseFile(entries.ElementAt(option - 1));
            }

            return entries.ElementAt(option - 1);
        }
    }
}
