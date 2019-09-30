using Gestion.Model;
using Gestion.Services;
using Subarashii.Core;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gestion.Srv
{
    public class ValuesController : Controller
    {

        private StudentService StudentService { get; set; }
        public ValuesController()
        {
            StudentService = new StudentService
                (Context.SubjectRepository, Context.StudentRepository);
        }

        [Handler("23")]
        public void ExecuteOrder23(string message, string auth)
        {
            Console.WriteLine("Received order 23");
            Console.WriteLine(message);
            Console.WriteLine(auth);
            Text("General Kenobi");
        }

        [Handler("66")]
        public void ExecuteOrder66(Student user, string auth)
        {
            Console.WriteLine("Received order 66");
            Console.WriteLine($"{user.Id} - {user.FirstName} {user.LastName}");
            Console.WriteLine(auth);
            Text("It\'s not the jedi way");
        }

        [Handler("77")]
        public void ExecuteOrder77(string file, string auth)
        {
            Console.WriteLine("Received order 77");
            Console.WriteLine(file);
            Text("Ok");
        }

        [Handler("88")]
        public void ExecuteOrder88(string uselessMsg, string auth)
        {
            Console.WriteLine("Received order 88");
            File(@"c:\Users\alvar\Pictures\tenor.gif");
        }

        [Handler("66")]
        public void ExecuteOrder66()
        {
            Console.WriteLine("Doing admin things");
        }

        [Handler("1")]
        public void ShowAllSubjects(string auth)
        {

            foreach (KeyValuePair<string, string> var in StudentService.GetEnrolledSubjects(auth))
            {
                Console.WriteLine(var.Key + " " + var.Value + " : Inscripto");
            }
            foreach (KeyValuePair<string, string> var in StudentService.GetNotEnrolledSubjects(auth))
            {
                Console.WriteLine(var.Key + " " + var.Value + ": No Inscripto");
            }
        }

        [Handler("2")]
        public void Enroll(string auth)
        {

            Dictionary<String, String> notEnrolled =
                StudentService.GetNotEnrolledSubjects(auth);
            foreach (KeyValuePair<string, string> var in notEnrolled)
            {
                Console.WriteLine(var.Key + " " + var.Value);
            }
            Console.WriteLine("Escriba el numero de la materia a la que se quiere inscribir");

            while (true)
            {
                string algo = Console.ReadLine();
                if (notEnrolled.ContainsKey(algo))
                {
                    StudentService.EnrollInSubject(auth, algo);
                    break;
                }
                else
                {
                    Console.WriteLine("Por favor, ingrese el numero de una de las materias que ve ahi");
                }
            }
        }
        [Handler("3")]
        public void LeaveSubject(string auth)
        {
            Dictionary<String, String> enrolled =
                StudentService.GetEnrolledSubjects(auth);
            foreach (KeyValuePair<string, string> var in enrolled)
            {
                Console.WriteLine(var.Key + " " + var.Value);
            }
            Console.WriteLine("Escriba el numero de la materia a la que se quiere desinscribir");
            while (true)
            {
                string algo = Console.ReadLine();
                if (enrolled.ContainsKey(algo))
                {
                    StudentService.AbandonSubject(auth, algo);
                    break;
                }
                else
                {
                    Console.WriteLine("Por favor, ingrese el numero de una de las materias que ve ahi");
                }
            }
        }

        [Handler("4")]
        public void UploadFileToSubject(string auth)
        {
            Dictionary<String, String> enrolled =
                StudentService.GetEnrolledSubjects(auth);
            foreach (KeyValuePair<string, string> var in enrolled)
            {
                Console.WriteLine(var.Key + " " + var.Value);
            }
            Console.WriteLine("Escriba el numero de la materia a la que desea subir un archivo");
            while (true)
            {
                string algo = Console.ReadLine();
                if (enrolled.ContainsKey(algo))
                {
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
                        var fr = new FileRef
                        {
                            Name = name,
                            Path = sourceFile,
                            Id = rnd.Next(1, 4000).ToString(),
                        };
                        StudentService.AddFileToSubject(fr, algo, auth);
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Por favor, ingrese el numero de una de las materias que ve ahi");
                }
            }
        }
        [Handler("5")]
        public void PrintGrades(string auth)
        {
            foreach (KeyValuePair<string, int?> var in StudentService.GetGrades(auth))
            {
                Console.WriteLine(var.Key + ", " + "Nota" + " : " + var.Value);
            }
        }
    }


}
