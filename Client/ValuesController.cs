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
        public void ShowSubjects()
        {
            Client.Send("1", "Eh Stan Lee");
            var response = Client.Receive();
            Console.WriteLine(response);
        }

       
    }
}
