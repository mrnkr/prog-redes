using Model.Exceptions;
using Subarashii.Repository;
using SubarashiiDemo.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Subarashii.Services.Exceptions;

namespace Subarashii.Services
{
    public class StudentService
    {
        StudentRepository StudentRepo;
        SubjectRepository SubjectRepo;

        public StudentService(SubjectRepository subRepo, StudentRepository studRepo)
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

        public void AddFileToSubject(FileRef file, Subject subject, Student student)
        {
            Student toMod = StudentRepo.GetAll().Find(s => s.Id == subject.Id);
            AddFileToDictionary(file, subject, toMod);
            StudentRepo.Modify(toMod);
        }

        private void AddFileToDictionary(FileRef file, Subject subject, Student student)
        {
            Dictionary<string, List<FileRef>> filesTemp = student.Files;
            List<FileRef> fileRefs;
            if (filesTemp.TryGetValue(subject.Id, out fileRefs))
            {
                fileRefs.Add(file);
            }
            else
            {
                throw new NotEnrolledToSubjectException();
            }
            student.Files.Add(subject.Id, fileRefs);
        }

        private void AddToSubjectList(Student student, Subject subject)
        {
            student.Grades.Add(subject.Id, null);
            student.Files.Add(subject.Id, null);
        }

        
    }
}
