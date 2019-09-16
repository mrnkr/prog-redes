using System;
using System.Collections.Generic;
using System.Text;
using SubarashiiDemo.Model;

namespace SubarashiiDemo.BusinessLogic
{
    public class StudentRepo : IRepo<Student>
    {
        private List<Student> Students;
        public void Add(Student ObjectToCreate)
        {
            Students.Add(ObjectToCreate);
        }

        public void Delete(Student ObjectToDelete)
        {
            Students.Remove(ObjectToDelete);
        }

        public void Modify(Student OldObject, Student NewObject)
        {
           //TODO. MIGHT HAVE TO THING THOROUGHLY BEFORE CODING.
        }

        public List<Student> GetAll()
        {
            return Students;
        }
    }
}
