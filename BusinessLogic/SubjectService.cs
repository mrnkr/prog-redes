using System;
using System.Collections.Generic;
using System.Text;
using Subarashii.Repository;
using SubarashiiDemo.Model;
using Subarashii.Services.Exceptions;

namespace Subarashii.Services
{
    public class SubjectBusinessLogic
    {
        SubjectRepository Subjects {get; set; }
        StudentRepository Students {get; set; }
        
        public SubjectBusinessLogic(SubjectRepository subjectRepo, StudentRepository studentRepo)
        {
            Subjects = subjectRepo;
            Students = studentRepo;
        }

        public void AssignGradeToStudent(Subject subject, Student student, int grade)
        {
            if (!ActiveSubject(subject.Id))
                throw new InactiveSubjectException();
            if (!StudentHasUploadedAFileToTheSubject(subject.Id, student.Id))
                throw new NoFilesInSubjectException();
            if (!StudentEnlistedInTheSubject(subject.Id, student.Id))
                throw new NotEnlistedException();
            Students.GetAll().Find(s => s.Id == student.Id).Grades.Add(subject.Id, grade);
        }

        private bool ActiveSubject(string subjectId)
        {
            return Subjects.GetAll().Find(s => s.Id == subjectId).IsActive;
        }

        private bool StudentHasUploadedAFileToTheSubject(string subjectId, string studentId)
        {
            bool hasFiles = true;
            Student stud = Students.GetAll().Find(s => s.Id == studentId);
            try
            {
                stud.Files.TryGetValue(subjectId, out List<FileRef> files);
            } catch (ArgumentNullException)
            {
                hasFiles = false;
            }
            return hasFiles;
        }
        private bool StudentEnlistedInTheSubject(string subjectId, string studentId)
        {
            Student stud = Students.GetAll().Find(s => s.Id == studentId);
            return stud.Grades.ContainsKey(subjectId);
        }
    }
}
