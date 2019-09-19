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

        public void Modify(Subject obj)
        {
            Subject toModify = Subjects.Find(v => v.Id == obj.Id);
            Subjects.Remove(toModify);
            Subjects.Add(obj);
        }
    }
}
