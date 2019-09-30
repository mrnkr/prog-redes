using Gestion.Model;
using Gestion.Model.Exceptions;
using Gestion.Repository;
using Gestion.Services.Exceptions;
using System;
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

        public void AbandonSubject(string studentId, string subjectId)
        {
            if (!SubjectRepo.GetAll().Find(s => s.Id == subjectId).IsActive)
                throw new InactiveSubjectException();
            Student studentToModify = StudentRepo.GetAll().Find(s => s.Id == studentId);
            Subject subjectToRemove = SubjectRepo.GetAll().Find(s => s.Id == subjectId);
            studentToModify.Files.Remove(subjectId);
            studentToModify.Grades.Remove(subjectId);
            StudentRepo.Modify(studentToModify);
        }

        public void AddFileToSubject(FileRef file, string subjectId, string studentId)
        {
            Student toMod = StudentRepo.GetAll().Find(s => s.Id == studentId);
            AddFileToDictionary(file, subjectId, toMod.Id);
            StudentRepo.Modify(toMod);
        }

        public Dictionary<string, string> GetEnrolledSubjects(string StudentId)
        {
            Dictionary<string,string> ret = new Dictionary<string, string>();
            Student toWatch = StudentRepo.GetAll().Find(s => s.Id == StudentId);
            Dictionary<string, int?>.KeyCollection grades = toWatch.Grades.Keys;
            foreach (Subject t in SubjectRepo.GetAll())
            {
                if (toWatch.Grades.ContainsKey(t.Id))
                {
                    ret.Add(t.Id,t.Name);
                }
            }
            return ret;
        }

        public Dictionary<string, string> GetNotEnrolledSubjects(string StudentId)
        {

            Dictionary<string, string> ret = new Dictionary<string, string>();
            Student toWatch = StudentRepo.GetAll().Find(s => s.Id == StudentId);
            Dictionary<string, int?>.KeyCollection grades = toWatch.Grades.Keys;
            foreach (Subject t in SubjectRepo.GetAll())
            {
                if (!toWatch.Grades.ContainsKey(t.Id))
                {
                    ret.Add(t.Id, t.Name); 
                }
            }
            return ret;
        }

        public Dictionary<string,int?> GetGrades (string StudentId)
        {
            Dictionary<string,int?> grades = 
                StudentRepo.GetAll().Find(s => s.Id == StudentId).Grades;
            Dictionary<string, int?> gradesAndSubjects = new Dictionary<string, int?>();
            foreach (KeyValuePair<string, int?> var in grades)
            {
                if (var.Value != null)
                {
                    gradesAndSubjects.Add(var.Key,var.Value);
                }
            }
            return gradesAndSubjects;
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
