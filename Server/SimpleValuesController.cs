using Gestion.Common;
using Gestion.Model;
using Gestion.Model.Exceptions;
using Gestion.Services;
using SimpleRouter;
using Subarashii.Core;
using System;
using System.Linq;

namespace Gestion.Srv
{
    public class SimpleValuesController : SimpleController
    {
        private Server Srv { get; }
        private IStudentService StudentSrv { get; }
        private ISubjectService SubjectSrv { get; }

        public SimpleValuesController(Server srv)
        {
            Srv = srv;
            StudentSrv = Context.GetInstance().StudentService;
            SubjectSrv = Context.GetInstance().SubjectService;
        }

        [SimpleHandler("1", "Registrar nuevo estudiante")]
        public void SignupStudent()
        {
            Console.Clear();
            ConsolePrompts.PrintHeader("Registro de estudiante");
            ConsolePrompts.PrintEmptyLine();

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

            ConsolePrompts.PrintEmptyLine();

            try
            {
                StudentSrv.SignupStudent(student);
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
            ConsolePrompts.PrintHeader("Registro de curso");
            ConsolePrompts.PrintEmptyLine();

            var name = ConsolePrompts.ReadUntilValid(
                prompt: "Nombre",
                pattern: "^[a-zA-Z0-9 ]+$",
                errorMsg: "El nombre de la materia puede tener letras de la A a la Z, no importan mayusculas, numeros o espacios. No se aceptan otros caracteres");

            var subject = new Subject()
            {
                Name = name
            };

            SubjectSrv.RegisterSubject(subject);
            ConsolePrompts.PrintEmptyLine();
        }

        [SimpleHandler("3", "Borrar materia")]
        public void RemoveSubject()
        {
            Console.Clear();
            ConsolePrompts.PrintHeader("Eliminacion de curso");
            ConsolePrompts.PrintEmptyLine();

            var subjects = SubjectSrv.GetAllSubjects();

            if (subjects.Count() == 0)
            {
                Console.WriteLine("No hay cursos registrados en el sistema");
                return;
            }

            ConsolePrompts.PrintListWithIndices(subjects.Select(s => s.Name));

            ConsolePrompts.PrintEmptyLine();
            var option = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Numero de la materia a borrar",
                min: 1,
                max: subjects.Count());

            var subject = subjects.ElementAt(option - 1);
            SubjectSrv.RemoveSubject(subject.Id);
            ConsolePrompts.PrintEmptyLine();
        }

        [SimpleHandler("4", "Ver cursos disponibles")]
        public void ViewAvailableCourses()
        {
            Console.Clear();
            ConsolePrompts.PrintHeader("Lista de cursos");
            ConsolePrompts.PrintEmptyLine();

            var subjects = SubjectSrv.GetAllSubjects();

            if (subjects.Count() == 0)
            {
                Console.WriteLine("No hay cursos registrados en el sistema");
                return;
            }

            ConsolePrompts.PrintListWithIndices(subjects.Select(s => s.Name));
            ConsolePrompts.PrintEmptyLine();
        }

        [SimpleHandler("5", "Calificar alumno")]
        public void GradeStudent()
        {
            Console.Clear();
            ConsolePrompts.PrintHeader("Asistente de calificaciones");
            ConsolePrompts.PrintEmptyLine();

            var subjects = SubjectSrv.GetAllSubjects();

            if (subjects.Count() == 0)
            {
                Console.WriteLine("No hay cursos registrados en el sistema");
                return;
            }

            Console.WriteLine("Materias disponibles");
            ConsolePrompts.PrintEmptyLine();
            ConsolePrompts.PrintListWithIndices(subjects.Select(s => s.Name));
            ConsolePrompts.PrintEmptyLine();

            var option = ConsolePrompts.ReadNumberUntilValid(
                prompt: "Numero de la materia deseada",
                min: 1,
                max: subjects.Count());

            var subject = subjects.ElementAt(option - 1);
            var students = StudentSrv.GetStudentsEnrolledInSubject(subject.Id);

            ConsolePrompts.PrintEmptyLine();

            if (students.Count() == 0)
            {
                Console.WriteLine($"No hay estudiantes inscriptos a {subject.Name}");
                return;
            }

            Console.WriteLine("Estudiantes inscriptos");
            ConsolePrompts.PrintEmptyLine();
            ConsolePrompts.PrintListWithIndices(students.Select(s => $"{s.LastName}, {s.FirstName} [{s.Id}]"));
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

            StudentSrv.GradeStudent(student.Id, subject.Id, grade);
            Srv.SendNotification(student.Id, $"Has recibido una calificacion! Sacaste {grade} en {subject.Name}");
            ConsolePrompts.PrintEmptyLine();
            Console.WriteLine($"Se notifico a {student.LastName}, {student.FirstName} que su nota para {subject.Name} fue {grade}");
            ConsolePrompts.PrintEmptyLine();
        }
    }
}
