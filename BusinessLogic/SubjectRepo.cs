using System;
using System.Collections.Generic;
using System.Text;
using SubarashiiDemo.Model;

namespace SubarashiiDemo.BusinessLogic
{
    public class SubjectRepo : IRepo<Subject>
    {
        List<Subject> Subjects;
        public void Add(Subject ObjectToCreate)
        {
            Subjects.Add(ObjectToCreate);
        }

        public void Delete(Subject ObjectToDelete)
        {
            Subjects.Remove(ObjectToDelete);
        }

        public List<Subject> GetAll()
        {
            return Subjects;
        }

        public void Modify(Subject OldObject, Subject NewObject)
        {
            Subject toModify = Subjects.Find(v => v.Id == OldObject.Id);
            toModify.Name = NewObject.Name;
        }
    }
}
