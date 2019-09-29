using Gestion.Model;
using Gestion.Model.Exceptions;
using Gestion.Repository;
using Gestion.Services.Exceptions;
using System.Collections.Generic;

namespace Gestion.Services
{
    public class StudentService
    {
        private IRepository<Student> StudentRepo { get; set; }
        private IRepository<Subject> SubjectRepo { get; set; }

        public StudentService(IRepository<Subject> subRepo, IRepository<Student> studRepo)
        {
            StudentRepo = studRepo;
            SubjectRepo = subRepo;
        }

        public void EnrollInSubject(string studentId, string subjectId)
        {
            if (!SubjectRepo.GetAll().Find(s => s.Id == subjectId).IsActive)
                throw new InactiveSubjectException();
            Student studentToModify = StudentRepo.GetAll().Find(s => s.Id == studentId);
            Subject subjectToAdd = SubjectRepo.GetAll().Find(s => s.Id == subjectId);
            AddToSubjectList(studentToModify, subjectToAdd);
            StudentRepo.Modify(studentToModify);
        }

        public void AddFileToSubject(FileRef file, string subjectId, string studentId)
        {
            Student toMod = StudentRepo.GetAll().Find(s => s.Id == studentId);
            AddFileToDictionary(file, subjectId, toMod.Id);
            StudentRepo.Modify(toMod);
        }

        private void AddFileToDictionary(FileRef file, string subjectId, string studentId)
        {
            Student stud = StudentRepo.GetAll().Find(s => s.Id == studentId);
            Dictionary<string, List<FileRef>> filesTemp = stud.Files;
            if (!stud.Files.ContainsKey(subjectId))
            {
                throw new UndefinedSubjectException();
            }
            List<FileRef> fileRefs;
            filesTemp.TryGetValue(subjectId, out fileRefs);
            if (fileRefs == null)
            {
                fileRefs = new List<FileRef>();
                fileRefs.Add(file);
            }
            stud.Files.Remove(subjectId);
            stud.Files.Add(subjectId, fileRefs);
        }

        private void AddToSubjectList(Student student, Subject subject)
        {
            student.Grades.Add(subject.Id, null);
            student.Files.Add(subject.Id, null);
        }

    }
}
