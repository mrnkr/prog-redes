using System;
using System.Collections.Generic;
using System.Text;
using Model.Exceptions;
using SubarashiiDemo.Model;

namespace SubarashiiDemo.BusinessLogic
{
    public class SubjectRepository : IRepository<Subject>
    {
        private List<Subject> Subjects = new List<Subject>();
        public void Add(Subject objectToCreate)
        {
            Subjects.Add(objectToCreate);
        }

        public void Delete(Subject objectToDelete)
        {
            if (!Exists(objectToDelete.Id))
            {
                throw new NonExistentSubjectException();
            }
            Subjects.Remove(objectToDelete);
        }

        public List<Subject> GetAll()
        {
            return Subjects;
        }

        public void Modify(Subject obj)
        {
            if (!Exists(obj.Id))
            {
                throw new NonExistentSubjectException();
            }
            Subject toModify = Subjects.Find(v => v.Id == obj.Id);
            Subjects.Remove(toModify);
            Subjects.Add(obj);
        }

        private bool Exists(string id)
        {
            return Subjects.Exists(s => s.Id == id);
        }
    }
}
