using Gestion.Model;
using Gestion.Repository;
using System;
using System.Collections.Generic;

namespace Gestion.Services.Impl
{
    public class SubjectService : MarshalByRefObject, ISubjectService
    {
        private IRepository<Subject> SubjectRepo { get; }
        private ILogger Logger { get; }
        
        public SubjectService(IRepository<Subject> subjectRepo, ILogger logger)
        {
            SubjectRepo = subjectRepo;
            Logger = logger;
        }

        public void RegisterSubject(Subject s)
        {
            SubjectRepo.Add(s);
            Logger.Log(EventType.SubjectRegistration, $"{s.Name} was registered");
        }

        public void RemoveSubject(string subjectId)
        {
            var s = SubjectRepo.Get(subjectId);
            SubjectRepo.Remove(s);
            Logger.Log(EventType.SubjectDeletion, $"{s.Name} was deleted");
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
