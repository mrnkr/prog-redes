using System;
using System.Collections.Generic;
using System.Text;
using SubarashiiDemo.Model;

namespace SubarashiiDemo.BusinessLogic
{
    public class StudentRepository : IRepository<Student>
    {
        private List<Student> Students;
        public void Add(Student objectToCreate)
        {
            Students.Add(objectToCreate);
        }

        public void Delete(Student ObjectToDelete)
        {
            Students.Remove(ObjectToDelete);
        }

        public void Modify(Student obj)
        {
            Student stud = Students.Find(p => p.Id == obj.Id);
            Students.Remove(stud);
            Students.Add(obj);
        }

        public List<Student> GetAll()
        {
            return Students;
        }
    }
}
