using Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion.Srv
{
    public class Context
    {

        public static StudentRepository StudentRepository { get; set; } = new StudentRepository();
        public static SubjectRepository SubjectRepository { get; set; } = new SubjectRepository();

    }
}
