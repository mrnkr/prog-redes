using Gestion.Model;
using Gestion.Model.Exceptions;
using System.Collections.Generic;

namespace Gestion.Repository
{
    public class SubjectRepository : IRepository<Subject>
    {
        private List<Subject> Subjects = new List<Subject>();
        private static SubjectRepository Instance { get; set; }

        private SubjectRepository()
        {
            Subjects = new List<Subject>();
        }

        public static SubjectRepository GetInstance()
        {
            if (Instance == null)
            {
                Instance = new SubjectRepository();
            }
            return Instance;
        }
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
            Subjects.Find(s => s.Id == objectToDelete.Id).IsActive = false;
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
