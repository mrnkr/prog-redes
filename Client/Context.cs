using Gestion.Repository;
using Gestion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion.Cli
{
    public class Context
    {
        public static string Auth { get; set; }

        public static StudentService StudentService { get; set; } 
            = new StudentService(new SubjectRepository(), new StudentRepository());

        public Context(string auth)
        {
            Auth = auth;
        }
    }
}
