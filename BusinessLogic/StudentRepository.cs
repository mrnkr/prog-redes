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

        public void Modify(Student oldObject, Student newObject)
        {
            Dictionary<Subject, int> tempGrades = oldObject.Grades;
            Dictionary<Subject, List<FileRef>> tempFiles = oldObject.Files;
            Students.Remove(oldObject);
            newObject.Files = tempFiles;
            newObject.Grades = tempGrades;
            Students.Add(newObject);
        }

        public List<Student> GetAll()
        {
            return Students;
        }
    }
}
