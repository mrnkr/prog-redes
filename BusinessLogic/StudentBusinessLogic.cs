using Model.Exceptions;
using SubarashiiDemo.BusinessLogic;
using SubarashiiDemo.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public class StudentBusinessLogic
    {
        StudentRepository studentRepo = new StudentRepository();
        SubjectRepository subjectRepo = new SubjectRepository();

        public void EnrollIntoSubject(string studentId, string subjectId)
        {
            Student studentToModify = studentRepo.GetAll().Find(s => s.Id == studentId);
            Subject subjectToAdd = subjectRepo.GetAll().Find(s => s.Id == subjectId);
            AddIntoSubjectList(studentToModify, subjectToAdd);
            studentRepo.Modify(studentToModify);
        }

        public void AddFileToSubject(FileRef file, Subject subject, Student student)
        {
            Student toMod = studentRepo.GetAll().Find(s => s.Id == subject.Id);
            AddFileToDictionary(file, subject, toMod);
            studentRepo.Modify(toMod);
        }

        private void AddFileToDictionary(FileRef file, Subject subject, Student student)
        {
            Dictionary<Subject, List<FileRef>> filesTemp = student.Files;
            List<FileRef> fileRefs;
            if (filesTemp.TryGetValue(subject, out fileRefs))
            {
                fileRefs.Add(file);
            }
            else
            {
                throw new NotEnrolledToSubjectException();
            }
            student.Files.Add(subject, fileRefs);
        }

        private void AddIntoSubjectList(Student student, Subject subject)
        {
            student.Grades.Add(subject, null);
            student.Files.Add(subject, null);
        }

        
    }
}
