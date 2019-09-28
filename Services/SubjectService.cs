using Gestion.Model;
using Gestion.Repository;
using Gestion.Services.Exceptions;
using System;
using System.Collections.Generic;

namespace Gestion.Services
{
    public class SubjectService
    {
        IRepository<Subject> SubjectRepo { get; set; }
        IRepository<Student> StudentRepo { get; set; }
        
        public SubjectService(IRepository<Subject> subjectRepo, IRepository<Student> studentRepo)
        {
            SubjectRepo = subjectRepo;
            StudentRepo = studentRepo;
        }

        public void AssignGradeToStudent(Subject subject, Student student, int grade)
        {
            if (!ActiveSubject(subject.Id))
                throw new InactiveSubjectException();
            if (!StudentHasUploadedAFileToTheSubject(subject.Id, student.Id))
                throw new NoFilesInSubjectException();
            if (!StudentEnlistedInTheSubject(subject.Id, student.Id))
                throw new NotEnlistedException();
            StudentRepo.GetAll().Find(s => s.Id == student.Id).Grades.Add(subject.Id, grade);
        }

        private bool ActiveSubject(string subjectId)
        {
            return SubjectRepo.GetAll().Find(s => s.Id == subjectId).IsActive;
        }

        private bool StudentHasUploadedAFileToTheSubject(string subjectId, string studentId)
        {
            bool hasFiles = true;
            Student stud = StudentRepo.GetAll().Find(s => s.Id == studentId);
            try
            {
                stud.Files.TryGetValue(subjectId, out List<FileRef> files);
            }
            catch (ArgumentNullException)
            {
                hasFiles = false;
            }
            return hasFiles;
        }

        private bool StudentEnlistedInTheSubject(string subjectId, string studentId)
        {
            Student stud = StudentRepo.GetAll().Find(s => s.Id == studentId);
            return stud.Grades.ContainsKey(subjectId);
        }
    }
}
