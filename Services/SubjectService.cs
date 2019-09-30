using Gestion.Model;
using Gestion.Repository;
using Gestion.Services.Exceptions;
using System.Collections.Generic;

namespace Gestion.Services
{
    public class SubjectService
    {
        private IRepository<Subject> SubjectRepo { get; set; }
        private IRepository<Student> StudentRepo { get; set; }
        
        public SubjectService(IRepository<Subject> subjectRepo, IRepository<Student> studentRepo)
        {
            SubjectRepo = subjectRepo;
            StudentRepo = studentRepo;
        }

        public void AssignGradeToStudent(string subjectId, string studentId, int grade)
        {
            if (!ActiveSubject(subjectId))
                throw new InactiveSubjectException();
            if (!StudentEnrolledInTheSubject(subjectId, studentId))
                throw new NotEnrolledException();
            if (!StudentHasUploadedAFileToTheSubject(subjectId, studentId))
                throw new NoFilesInSubjectException();
            
            StudentRepo.GetAll().Find(s => s.Id == studentId).Grades.Remove(subjectId);
            StudentRepo.GetAll().Find(s => s.Id == studentId).Grades.Add(subjectId,grade);
        }

        public void UploadFile(string subjectId,FileRef file)
        {
            Subject sub = SubjectRepo.GetAll().Find(s => s.Id == subjectId);
            sub.Files.Add(file);
            SubjectRepo.Modify(sub);

        }
        private bool ActiveSubject(string subjectId)
        {
            return SubjectRepo.GetAll().Find(s => s.Id == subjectId).IsActive;
        }

        private bool StudentHasUploadedAFileToTheSubject(string subjectId, string studentId)
        {
            bool hasFiles = true;
            Student stud = StudentRepo.GetAll().Find(s => s.Id == studentId);
            List<FileRef> files;    
            stud.Files.TryGetValue(subjectId, out files);
            if (files == null)
            {
                hasFiles = false;
            }
            return hasFiles;
        }

        private bool StudentEnrolledInTheSubject(string subjectId, string studentId)
        {
            Student stud = StudentRepo.GetAll().Find(s => s.Id == studentId);
            return stud.Files.ContainsKey(subjectId);
        }
    }
}
