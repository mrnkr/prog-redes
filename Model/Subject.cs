using SubarashiiDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subarashii.Model
{
    public class Subject
    {
        public string Name { get; set; }
        public Dictionary<User, int> StudentsAndNotes { get; set; }
        public string SubjectId { get; set; }

    }
}
