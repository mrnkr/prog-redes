using SubarashiiDemo.Model;
using System.Collections;
using System.Collections.Generic;

namespace SubarashiiDemo.Model
{
    public class Student {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Dictionary<Subject, int> Grades { get; set; }

        public Dictionary<Subject, List<FileRef>> Files {get;set;} 
    }
}