using System;
using System.Collections.Generic;
using System.Text;
using SubarashiiDemo.Model;

namespace SubarashiiDemo.BusinessLogic
{
    public class SubjectRepository : IRepository<Subject>
    {
        private List<Subject> Subjects;
        public void Add(Subject objectToCreate)
        {
            Subjects.Add(objectToCreate);
        }

        public void Delete(Subject ObjectToDelete)
        {
            Subjects.Remove(ObjectToDelete);
        }

        public List<Subject> GetAll()
        {
            return Subjects;
        }

        public void Modify(Subject oldObject, Subject newObject)
        {
            Subject toModify = Subjects.Find(v => v.Id == oldObject.Id);
            toModify.Name = newObject.Name;
        }
    }
}
