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

        public void AddFileToSubject(FileRef file, Subject sub, Student stud)
        {
            Student toMod = studentRepo.GetAll().Find(s => s.Id == sub.Id);
            AddFileToDictionary(file, sub, toMod);
            studentRepo.Modify(toMod);
        }

        private void AddFileToDictionary(FileRef file, Subject subj, Student stud)
        {
            Dictionary<Subject, List<FileRef>> filesTemp = stud.Files;
            List<FileRef> fileRefs;
            if (filesTemp.TryGetValue(subj, out fileRefs))
            {
                fileRefs.Add(file);
            }
            else
            {
                throw new NotEnrolledToSubjectException();
            }
            stud.Files.Add(subj, fileRefs);
        }

        private void AddIntoSubjectList(Student stud, Subject sub)
        {
            stud.Grades.Add(sub, null);
            stud.Files.Add(sub, null);
        }

        
    }
}
