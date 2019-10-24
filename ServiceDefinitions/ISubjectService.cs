using Gestion.Model;
using System.Collections.Generic;

namespace Gestion.Services
{
    public interface ISubjectService
    {
        void RegisterSubject(Subject s);
        void RemoveSubject(string subjectId);
        IEnumerable<Subject> GetAllSubjects();
        Subject GetSubjectById(string subjectId);
    }
}
