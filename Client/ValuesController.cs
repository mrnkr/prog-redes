using Gestion.Model;
using Helpers;
using SimpleRouter;
using Subarashii.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Cli
{
    public class ValuesController : SimpleController
    {
        private Client Client { get; }

        public ValuesController(Client c)
        {
            Client = c;
        }

        [SimpleHandler("1", "Anotarse en un curso")]
        public void EnrollInSubject()
        {
            Console.Clear();
            Console.WriteLine("Cargando...");
            Client.Send("03", "NOT_MINE");
            var subjects = Client.Receive<IEnumerable<Subject>>();
            Console.Clear();

            Console.WriteLine("Inscripciones");
            Console.WriteLine("-------------");
            Console.WriteLine("");

            if (subjects.Count() == 0)
            {
                Console.WriteLine("No hay cursos a los que se pueda registrar");
                return;
            }

            ConsolePrompts.PrintListWithIndices(subjects.Select(s => s.Name));

            Console.WriteLine("");
            var option = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Numero de la materia a la que se quiere inscribir",
                min: 1,
                max: subjects.Count());

            Client.Send("04", subjects.ElementAt(option - 1).Id);
            var result = Client.Receive();

            if (result != "OK")
            {
                Console.WriteLine("Ha ocurrido un error, intente nuevamente");
                return;
            }
            
            Console.WriteLine("Esta usted inscripto");
        }

        [SimpleHandler("2", "Ver cursos disponibles")]
        public void ViewAvailableCourses()
        {
            Console.Clear();
            Console.WriteLine("Cargando...");
            Client.Send("03", "ALL_WITH_STATUS");
            var subjects = Client.Receive<IDictionary<string, string>>();
            Console.Clear();

            Console.WriteLine("Lista de cursos disponibles");
            Console.WriteLine("---------------------------");
            Console.WriteLine("");

            if (subjects.Count() == 0)
            {
                Console.WriteLine("No hay cursos registrados en el sistema");
                return;
            }

            ConsolePrompts.PrintListWithIndices(subjects.Select(s => $"{s.Key} ({s.Value})"));

            Console.WriteLine("");
        }

        [SimpleHandler("3", "Subir material a un curso")]
        public void UploadFileToCourse()
        {
            Console.Clear();
            Console.WriteLine("Cargando...");
            Client.Send("03", "MINE");
            var subjects = Client.Receive<IEnumerable<Subject>>();
            Console.Clear();

            Console.WriteLine("Lista de cursos disponibles");
            Console.WriteLine("---------------------------");
            Console.WriteLine("");

            if (subjects.Count() == 0)
            {
                Console.WriteLine("No esta inscripto a ningun curso");
                return;
            }

            ConsolePrompts.PrintListWithIndices(subjects.Select(s => s.Name));

            Console.WriteLine("");
            var option = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Numero de la materia a la que quiere subir material",
                min: 1,
                max: subjects.Count());

            var subject = subjects.ElementAt(option - 1);
            Console.WriteLine($"El material sera subido a {subject.Name}");

            var path = ConsolePrompts.ChooseFile();
            Client.SendFile("05", path);
            var fileRef = Client.Receive<FileRef>();

            Client.Send("06", Tuple.Create(subject.Id, fileRef));
            if (Client.Receive() != "OK")
            {
                Console.WriteLine("Ha ocurrido un error, intente nuevamente");
                return;
            }

            Console.WriteLine("Archivo subido con exito!");
        }

        [SimpleHandler("4", "Ver historial de notas")]
        public void ViewGrades()
        {
            Console.Clear();
            Console.WriteLine("Cargando...");
            Client.Send("07", "MINE");
            var grades = Client.Receive<IDictionary<string, int>>();
            Console.Clear();

            Console.WriteLine("Notas");
            Console.WriteLine("-----");
            Console.WriteLine("");

            if (grades.Count() == 0)
            {
                Console.WriteLine("No tienes ninguna nota aun");
                return;
            }

            ConsolePrompts.PrintListWithIndices(grades.Select(entry => $"{entry.Key} - {entry.Value}"));
            Console.WriteLine("");
        }
    }
}
