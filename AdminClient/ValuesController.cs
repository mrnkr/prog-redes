using Gestion.Admin.Cli.ViewModels;
using Gestion.Common;
using Gestion.Common.Exceptions;
using SimpleRouter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gestion.Admin.Cli
{
    public class ValuesController : SimpleController
    {
        private HttpClient Admin { get; }
        private HttpClient Logs { get; }

        public ValuesController(HttpClient admin, HttpClient logs)
        {
            Admin = admin;
            Logs = logs;
        }

        [SimpleHandler("1", "Calificar alumno")]
        public async Task GradeStudent()
        {
            Console.Clear();

            Console.WriteLine("Cargando...");
            var subjects = await Admin.GetObjectAsync<IEnumerable<SubjectViewModel>>("/api/subjects");
            Console.Clear();

            ConsolePrompts.PrintHeader("Asistente de calificaciones");
            ConsolePrompts.PrintEmptyLine();


            if (subjects.Count() == 0)
            {
                Console.WriteLine("No hay cursos registrados en el sistema");
                return;
            }

            Console.WriteLine("Materias disponibles");
            ConsolePrompts.PrintEmptyLine();
            ConsolePrompts.PrintListWithIndices(subjects.Select(s => s.name));
            ConsolePrompts.PrintEmptyLine();

            var option = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Numero de la materia deseada",
                min: 1,
                max: subjects.Count());

            var subject = subjects.ElementAt(option - 1);

            Console.WriteLine("Cargando...");
            ConsolePrompts.PrintEmptyLine();
            var students = await Admin.GetObjectAsync<StudentViewModel[]>($"/api/students?subjectId={subject.id}");

            ConsolePrompts.PrintEmptyLine();

            if (students.Count() == 0)
            {
                Console.WriteLine($"No hay estudiantes inscriptos a {subject.name}");
                return;
            }

            Console.WriteLine("Estudiantes inscriptos");
            ConsolePrompts.PrintEmptyLine();
            ConsolePrompts.PrintListWithIndices(students.Select(s => $"{s.lastName}, {s.firstName} [{s.id}]"));
            ConsolePrompts.PrintEmptyLine();

            option = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Numero del estudiante",
                min: 1,
                max: subjects.Count());

            var student = students.ElementAt(option - 1);

            ConsolePrompts.PrintEmptyLine();
            var grade = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Nota",
                min: 1,
                max: 100);

            Console.WriteLine("Cargando...");
            ConsolePrompts.PrintEmptyLine();
            await Admin.PostAndExpectObjectAsync<string>("/api/grading", new
            {
                studentId = student.id,
                subjectId = subject.id,
                grade
            });

            Console.WriteLine($"Se ha cargado la nota {grade} a {student.lastName}, {student.firstName}");
            ConsolePrompts.PrintEmptyLine();
        }

        [SimpleHandler("2", "Registrar profesor")]
        public async Task RegisterTeacher()
        {
            Console.Clear();
            ConsolePrompts.PrintHeader("Asistente de registro de profesores");
            ConsolePrompts.PrintEmptyLine();

            Console.WriteLine("Se le pedira llenar todos los campos para la creacion de un profesor");
            ConsolePrompts.PrintEmptyLine();

            var firstName = ConsolePrompts.ReadUntilValid(
                prompt: "Nombre",
                pattern: ".+",
                errorMsg: "El nombre no puede ser vacio");

            var lastName = ConsolePrompts.ReadUntilValid(
              prompt: "Apellido",
              pattern: ".+",
              errorMsg: "El apellido no puede ser vacio");

            var email = ConsolePrompts.ReadUntilValid(
              prompt: "Email",
              pattern: Constants.EMAIL_REGEX,
              errorMsg: "Email invalido");

            var password = ConsolePrompts.ReadUntilValid(
              prompt: "Password",
              pattern: ".+",
              errorMsg: "Password invalido");

            Console.WriteLine("Cargando...");
            ConsolePrompts.PrintEmptyLine();

            try
            {
                await Admin.PostAndExpectObjectAsync<string>("/api/signup", new
                {
                    firstName,
                    lastName,
                    email,
                    password,
                });
            }
            catch
            {
                throw new OperationFailedException();
            }
           

            Console.WriteLine($"Se ha registrado el profesor {lastName}, {firstName}");
            ConsolePrompts.PrintEmptyLine();
        }

        [SimpleHandler("3", "Logs")]
        public async Task ShowLogs()
        {
            var query = new QueryBuilder();

            Console.Clear();
            ConsolePrompts.PrintHeader("Logs");
            ConsolePrompts.PrintEmptyLine();

            ConsolePrompts.PrintListWithIndices(Constants.EventTypes);
            ConsolePrompts.PrintEmptyLine();

            var option = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Numero del tipo de evento deseado",
                min: 0,
                max: Constants.EventTypes.Count());

            if (option == 0)
            {
                var eventType = Constants.EventTypes.ElementAt(option - 1);
                query.WithEventType(eventType);
            }
            
            var sort = ConsolePrompts.ReadUntilValid(
                prompt: "Desea ordenar los eventos por fecha? [Y/n]",
                pattern: "Y|n",
                errorMsg: "Por favor, ingrese una Y o una n");

            if (sort == "Y")
            {
                var isDescending = ConsolePrompts.ReadUntilValid(
                    prompt: "Desea ordenar los eventos de forma descendente? [Y/n]",
                    pattern: "Y|n",
                    errorMsg: "Por favor, ingrese una Y o una n");

                if (isDescending == "Y")
                {
                    query.SortDescending();
                }
                else
                {
                    query.Sort();
                }
            }

            Console.WriteLine("Cargando...");
            try
            {
                var logs = await Logs.GetObjectAsync<IEnumerable<LogEntryViewModel>>($"/api/logs{query.Build()}");
                ConsolePrompts.PrintListWithIndices(logs.Select(l => $"[{l.timestamp}] {l.eventType} - {l.description}"));
            }
            catch
            {
                throw new OperationFailedException();
            }

            ConsolePrompts.PrintEmptyLine();
        }

    }
}
