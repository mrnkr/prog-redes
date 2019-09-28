using System.Collections.Generic;

namespace Gestion.Model
{
    public class Student {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Dictionary<string, int?> Grades { get; set; }

        public Dictionary<string, List<FileRef>> Files {get;set;} 
    }
}