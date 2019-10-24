using Gestion.Model;
using Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Services.Impl
{
    public class SubjectService : MarshalByRefObject, ISubjectService
    {
        private IRepository<Subject> SubjectRepo { get; set; }
        
        public SubjectService(IRepository<Subject> subjectRepo)
        {
            SubjectRepo = subjectRepo;
        }

        public void RegisterSubject(Subject s)
        {
            SubjectRepo.Add(s);
        }

        public void RemoveSubject(string subjectId)
        {
            var s = SubjectRepo.Get(subjectId);
            SubjectRepo.Remove(s);
        }

        public IEnumerable<Subject> GetAllSubjects()
        {
            var subjects = SubjectRepo.GetAll();
            return subjects;
        }

        public Subject GetSubjectById(string subjectId)
        {
            return SubjectRepo.Get(subjectId);
        }
    }
}
