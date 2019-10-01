using Gestion.Model;
using Gestion.Model.Exceptions;
using Helpers;
using SimpleRouter;
using Subarashii.Core;
using System;
using System.Linq;

namespace Gestion.Srv
{
    public class SimpleValuesController : SimpleController
    {
        private Server Srv { get; }

        public SimpleValuesController(Server srv)
        {
            Srv = srv;
        }

        [SimpleHandler("1", "Registrar nuevo estudiante")]
        public void SignupStudent()
        {
            var studentService = Context.GetInstance().StudentService;

            Console.Clear();
            Console.WriteLine("Registro de estudiante");
            Console.WriteLine("----------------------");
            Console.WriteLine("");

            var id = ConsolePrompts.ReadUntilValid(
                prompt: "Numero de estudiante",
                pattern: "^[0-9]{6}$",
                errorMsg: "Un numero de estudiante tiene solo seis numeros");
            var firstName = ConsolePrompts.ReadUntilValid(
                prompt: "Primer nombre",
                pattern: "^[a-zA-Z]+$",
                errorMsg: "Un nombre son letras de la A a la Z, no importan mayusculas. No se aceptan espacios ni ningun otro caracter");
            var lastName = ConsolePrompts.ReadUntilValid(
                prompt: "Primer apellido",
                pattern: "^[a-zA-Z]+$",
                errorMsg: "Un apellido son letras de la A a la Z, no importan mayusculas. No se aceptan espacios ni ningun otro caracter");

            var student = new Student()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName
            };

            Console.WriteLine("");

            try
            {
                studentService.SignupStudent(student);
            }
            catch (DuplicateEntityException)
            {
                Console.WriteLine("Un estudiante con ese numero ya existe, cancelando la operacion...");
            }
        }

        [SimpleHandler("2", "Registrar nueva materia")]
        public void RegisterSubject()
        {
            Console.Clear();
            Console.WriteLine("Registro de curso");
            Console.WriteLine("-----------------");
            Console.WriteLine("");

            var name = ConsolePrompts.ReadUntilValid(
                prompt: "Nombre",
                pattern: "^[a-zA-Z0-9 ]+$",
                errorMsg: "El nombre de la materia puede tener letras de la A a la Z, no importan mayusculas, numeros o espacios. No se aceptan otros caracteres");

            var subject = new Subject()
            {
                Name = name
            };

            var subjectService = Context.GetInstance().SubjectService;
            subjectService.RegisterSubject(subject);
            Console.WriteLine("");
        }

        [SimpleHandler("3", "Borrar materia")]
        public void RemoveSubject()
        {
            Console.Clear();
            Console.WriteLine("Eliminacion de curso");
            Console.WriteLine("--------------------");
            Console.WriteLine("");

            var subjectService = Context.GetInstance().SubjectService;
            var subjects = subjectService.GetAllSubjects();

            if (subjects.Count() == 0)
            {
                Console.WriteLine("No hay cursos registrados en el sistema");
                return;
            }

            ConsolePrompts.PrintListWithIndices(subjects.Select(s => s.Name));

            Console.WriteLine("");
            var option = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Numero de la materia a borrar",
                min: 1,
                max: subjects.Count());

            var subject = subjects.ElementAt(option - 1);
            subjectService.RemoveSubject(subject.Id);
            Console.WriteLine("");
        }

        [SimpleHandler("4", "Ver cursos disponibles")]
        public void ViewAvailableCourses()
        {
            Console.Clear();
            Console.WriteLine("Lista de cursos");
            Console.WriteLine("---------------");
            Console.WriteLine("");

            var subjectService = Context.GetInstance().SubjectService;
            var subjects = subjectService.GetAllSubjects();

            if (subjects.Count() == 0)
            {
                Console.WriteLine("No hay cursos registrados en el sistema");
                return;
            }

            ConsolePrompts.PrintListWithIndices(subjects.Select(s => s.Name));
            Console.WriteLine("");
        }

        [SimpleHandler("5", "Calificar alumno")]
        public void GradeStudent()
        {
            var studentService = Context.GetInstance().StudentService;
            var subjectService = Context.GetInstance().SubjectService;

            Console.Clear();
            Console.WriteLine("Asistente de calificaciones");
            Console.WriteLine("---------------------------");
            Console.WriteLine("");

            var subjects = subjectService.GetAllSubjects();

            if (subjects.Count() == 0)
            {
                Console.WriteLine("No hay cursos registrados en el sistema");
                return;
            }

            Console.WriteLine("Materias disponibles");
            Console.WriteLine("");
            ConsolePrompts.PrintListWithIndices(subjects.Select(s => s.Name));

            Console.WriteLine("");
            var option = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Numero de la materia deseada",
                min: 1,
                max: subjects.Count());

            var subject = subjects.ElementAt(option - 1);
            var students = studentService.GetStudentsEnrolledInSubject(subject.Id);

            Console.WriteLine("");

            if (students.Count() == 0)
            {
                Console.WriteLine($"No hay estudiantes inscriptos a {subject.Name}");
                return;
            }

            Console.WriteLine("Estudiantes inscriptos");
            Console.WriteLine("");
            ConsolePrompts.PrintListWithIndices(students.Select(s => $"{s.LastName}, {s.FirstName} [{s.Id}]"));

            Console.WriteLine("");
            option = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Numero del estudiante",
                min: 1,
                max: subjects.Count());

            var student = students.ElementAt(option - 1);

            Console.WriteLine("");
            var grade = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Nota",
                min: 1,
                max: 100);

            studentService.GradeStudent(student.Id, subject.Id, grade);
            Srv.SendNotification(student.Id, $"Has recibido una calificacion! Sacaste {grade} en {subject.Name}");
            Console.WriteLine("");
            Console.WriteLine($"Se notifico a {student.LastName}, {student.FirstName} que su nota para {subject.Name} fue {grade}");
        }
    }
}
