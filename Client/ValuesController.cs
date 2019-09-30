using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Gestion.Model;
using Gestion.Repository;
using Gestion.Services;
using SimpleRouter;
using Subarashii.Core;

namespace Gestion.Cli
{
    public class ValuesController : SimpleController
    {

        private StudentService StudentService { get; set; }
        private Client Client { get; }

        public ValuesController(Client c)
        {
            Client = c;
            StudentService = new StudentService(new SubjectRepository(), new StudentRepository());
        }

        [SimpleHandler("66")]
        public void ExecuteOrder66()
        {
            Client.Send("23", "Hello there");
            var response = Client.Receive();
            Console.WriteLine(response);
        }

        [SimpleHandler("1")]
        public void ShowAllSubjects()
        {

            foreach (KeyValuePair<string, string> var in StudentService.GetEnrolledSubjects(Context.Auth))
            {
                Console.WriteLine(var.Key + " " + var.Value + " : Inscripto");
            }
            foreach (KeyValuePair<string, string> var in StudentService.GetNotEnrolledSubjects(Context.Auth))
            {
                Console.WriteLine(var.Key + " " + var.Value + ": No Inscripto");
            }
        }

        [SimpleHandler("2")]
        public void Enroll()
        {

            Dictionary<String, String> notEnrolled =
                StudentService.GetNotEnrolledSubjects(Context.Auth);
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
                    StudentService.EnrollInSubject(Context.Auth, algo);
                    break;
                }
                else
                {
                    Console.WriteLine("Por favor, ingrese el numero de una de las materias que ve ahi");
                }
            }
        }
        [SimpleHandler("3")]
        public void LeaveSubject()
        {
            Dictionary<String, String> enrolled =
                StudentService.GetEnrolledSubjects(Context.Auth);
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
                    StudentService.AbandonSubject(Context.Auth, algo);
                    break;
                }
                else
                {
                    Console.WriteLine("Por favor, ingrese el numero de una de las materias que ve ahi");
                }
            }
        }

        [SimpleHandler("4")]
        public void UploadFileToSubject()
        {
            Dictionary<String, String> enrolled =
                StudentService.GetEnrolledSubjects(Context.Auth);
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
                        StudentService.AddFileToSubject(fr, algo, Context.Auth);
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Por favor, ingrese el numero de una de las materias que ve ahi");
                }
            }
        }
        [SimpleHandler("5")]
        public void PrintGrades()
        {
            foreach (KeyValuePair<string, int?> var in StudentService.GetGrades(Context.Auth))
            {
                Console.WriteLine(var.Key + ", " + "Nota" + " : " + var.Value);
            }
        }
    }
}
