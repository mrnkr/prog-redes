using Gestion.Model;
using Gestion.Repository;
using Gestion.Services;
using Gestion.Services.Exceptions;
using SimpleRouter;
using Subarashii.Core;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gestion.Srv
{
    public class SimpleValuesController : SimpleController
    {
        private Server Srv { get; }

        private StudentService StudentService { get; set; }

        private SubjectService SubjectService { get; set; }

        public SimpleValuesController(Server srv)
        {
            Srv = srv;
            StudentService = new StudentService(Context.SubjectRepository, Context.StudentRepository);
            SubjectService = new SubjectService(Context.SubjectRepository, Context.StudentRepository);
        }

        [SimpleHandler("1")]
        public void CreateSubject()
        {
            Console.WriteLine("Ingrese el nombre de la materia");
            string name = Console.ReadLine();
            Console.WriteLine("Ingrese la Id de la materia");
            string id = Console.ReadLine();
            var s = new Subject
            {
                Name = name,
                Id = id,
                IsActive = true,
                Files = new List<FileRef>(),
            };
            Context.SubjectRepository.Add(s);
            Console.WriteLine("Agregado correctamente");
        }

        [SimpleHandler("2")]
        public void CreateStudent()
        {
            Console.WriteLine("Ingrese el nombre del estudiante");
            string fname = Console.ReadLine();
            Console.WriteLine("Ingrese el apellido");
            string lname = Console.ReadLine();
            Console.WriteLine("Ingrese el numero de estudiante");
            string id = Console.ReadLine();
            var stud = new Student
            {
                FirstName = fname,
                LastName = lname,
                Id = id,
                Files = new Dictionary<string, List<FileRef>>(),
                Grades = new Dictionary<string, int?>(),
            };
            Context.StudentRepository.Add(stud);
            Console.WriteLine("Agregado correctamente");
        }

        [SimpleHandler("3")]
        public void PrintSubjects()
        {
            foreach (Subject var in Context.SubjectRepository.GetAll().FindAll(s => s.IsActive == true))
            {
                Console.WriteLine(var.Id + " , " + var.Name);
            }
        }

        [SimpleHandler("4")]
        public void PrintStudents()
        {
            foreach (Student var in Context.StudentRepository.GetAll())
            {
                Console.WriteLine(var.Id + "," + var.LastName + ", " + var.FirstName);
            }
        }

        [SimpleHandler("5")]
        public void DeleteSubject()
        {
            PrintSubjects();
            Console.WriteLine("Ingrese el numero de la materia que desea eliminar");
            string algo = Console.ReadLine();
            if (Context.SubjectRepository.GetAll().Exists(s => s.Id == algo))
            {
                Subject sub = Context.SubjectRepository.GetAll().Find(s => s.Id == algo);
                Context.SubjectRepository.Delete(sub);
                Console.WriteLine("Eliminado correctamente");
            }

        }
        [SimpleHandler("6")]
        public void DeleteStudent()
        {
            PrintStudents();
            Console.WriteLine("Ingrese el numero de estudiante que desea ver reducido a atomos");
            string algo = Console.ReadLine();
            if (Context.StudentRepository.GetAll().Exists(s => s.Id == algo))
            {
                Student sub = Context.StudentRepository.GetAll().Find(s => s.Id == algo);
                Context.StudentRepository.Delete(sub);
                Console.WriteLine("Reducido a atomos correctamente");
            }
        }
        [SimpleHandler("7")]
        public void AssignGradeToStudent()
        {
            Console.WriteLine("Estudiantes");
            PrintStudents();
            Console.WriteLine("Materias");
            PrintSubjects();
            Console.WriteLine("Seleccione el estudiante que desea evaluar");
            string student = Console.ReadLine();
            Console.WriteLine("Seleccione la materia");
            string subject = Console.ReadLine();
            Console.WriteLine("Asigne una nota");
            string grade = Console.ReadLine();
            int grad;
            Int32.TryParse(grade, out grad);
            try
            {
                SubjectService.AssignGradeToStudent(subject, student, grad);
            }
            catch (NoFilesInSubjectException)
            {
                Console.WriteLine("No hay archivos para evaluar");
            }
            catch (NotEnrolledException)
            {
                Console.WriteLine("Estudiante no esta en esa materia");
            }
        }

        [SimpleHandler("8")]
        public void AddFile()
        {
            PrintSubjects();
            Console.WriteLine("Seleccione la materia a la que desea subir un archivo");
            string subject = Console.ReadLine();
            FileRef fr = CreateFileRef();
            SubjectService.UploadFile(subject, fr);
        }

        private FileRef CreateFileRef()
        {
            FileRef file = new FileRef();
            OpenFileDialog opf = new OpenFileDialog();
            opf.InitialDirectory = @"C:\";
            opf.RestoreDirectory = true;
            opf.Title = "Busca un archivo para subir";
            opf.CheckFileExists = true;
            opf.CheckPathExists = true;
            DialogResult a = opf.ShowDialog();
            if (a == System.Windows.Forms.DialogResult.OK)
            {
                Random rnd = new Random();
                string sourceFile = opf.FileName;
                string name = opf.SafeFileName;
                file = new FileRef
                {
                    Name = name,
                    Path = sourceFile,
                    Id = rnd.Next(1, 4000).ToString(),
                };
            }
            return file;
        }
    }
}
