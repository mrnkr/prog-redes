using Gestion.Model;
using Gestion.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Services.Impl
{
    public class StudentService : MarshalByRefObject, IStudentService
    {
        private IRepository<Student> StudentRepo { get; set; }
        private IRepository<Subject> SubjectRepo { get; set; }

        public StudentService(IRepository<Subject> subRepo, IRepository<Student> studRepo)
        {
            StudentRepo = studRepo;
            SubjectRepo = subRepo;
        }

        public Student GetStudentById(string studentId)
        {
            return StudentRepo.Get(studentId);
        }

        public IEnumerable<Student> GetStudentsEnrolledInSubject(string subjectId)
        {
            return StudentRepo.Find(student => student.GetSubjects().Any(s => s.Id == subjectId));
        }

        public void SignupStudent(Student s)
        {
            StudentRepo.Add(s);
        }

        public IEnumerable<Subject> GetSubjectsStudentIsEnrolledIn(string studentId)
        {
            return GetStudentById(studentId).GetSubjects();
        }

        public IDictionary<string, string> GetSubjectsAndStatusAccordingToStudent(string studentId)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();

            var student = StudentRepo.Get(studentId);
            var subjects = SubjectRepo.GetAll();
            
            foreach (var s in subjects)
            {
                if (student.GetGrades().ContainsKey(s.Id))
                {
                    ret.Add(s.Name, "Con nota");
                    continue;
                }

                if (student.GetSubjects().Any(sub => sub.Id == s.Id))
                {
                    ret.Add(s.Name, "Inscripto");
                    continue;
                }

                ret.Add(s.Name, "No inscripto");
            }

            return ret;
        }

        public void EnrollInSubject(string studentId, string subjectId)
        {
            var student = StudentRepo.Get(studentId);
            var subject = SubjectRepo.Get(subjectId);

            student.AddSubject(subject);
            StudentRepo.Update(student);
        }

        public void LinkUploadedFileToSubjectForStudent(string studentId, string subjectId, FileRef file)
        {
            var student = StudentRepo.Get(studentId);
            student.AddFileToSubject(subjectId, file);
            StudentRepo.Update(student);
        }

        public IEnumerable<FileRef> GetFilesUploadedByStudent(string studentId, string subjectId)
        {
            var student = StudentRepo.Get(studentId);
            return student.GetFilesForSubject(subjectId);
        }

        public void GradeStudent(string studentId, string subjectId, int grade)
        {
            var student = StudentRepo.Get(studentId);
            student.AddGrade(subjectId, grade);
            StudentRepo.Update(student);
        }
    }
}
