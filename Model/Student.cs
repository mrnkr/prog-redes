using Gestion.Model.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Model
{
    public class Student : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private ICollection<Subject> Subjects { get; set; }
        private IDictionary<string, int> Grades { get; set; }
        private IDictionary<string, ICollection<FileRef>> Files { get; set; }

        public Student()
        {
            Subjects = new List<Subject>();
            Grades = new Dictionary<string, int>();
            Files = new Dictionary<string, ICollection<FileRef>>();
        }

        public IEnumerable<Subject> GetSubjects()
        {
            return Subjects;
        }

        public void AddSubject(Subject s)
        {
            Subjects.Add(s);
        }

        public IDictionary<string, int> GetGrades()
        {
            return Grades;
        }

        public void AddGrade(string subjectId, int grade)
        {
            AssertStudentIsEnrolledInSubject(subjectId);

            if (Grades.ContainsKey(subjectId))
            {
                Grades.Remove(subjectId);
            }

            Grades.Add(subjectId, grade);
        }

        public IEnumerable<FileRef> GetFilesForSubject(string subjectId)
        {
            if (Files.TryGetValue(subjectId, out var ret))
            {
                return ret;
            }
            else
            {
                return new List<FileRef>();
            }
        }

        public void AddFileToSubject(string subjectId, FileRef file)
        {
            AssertStudentIsEnrolledInSubject(subjectId);

            if (Files.TryGetValue(subjectId, out var list))
            {
                list.Add(file);
            }
            else
            {
                var lst = new List<FileRef>();
                lst.Add(file);
                Files.Add(subjectId, lst);
            }
        }

        private void AssertStudentIsEnrolledInSubject(string subjectId)
        {
            if (!Subjects.Select(s => s.Id).Contains(subjectId))
            {
                throw new StudentNotEnrolledException();
            }
        }

        public override object Clone()
        {
            var ret = new Student()
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
            };

            foreach (var subject in Subjects)
            {
                ret.AddSubject(subject);
            }

            foreach (var grade in Grades)
            {
                ret.AddGrade(grade.Key, grade.Value);
            }

            foreach (var subject in Files)
            {
                foreach (var file in subject.Value)
                {
                    ret.AddFileToSubject(subject.Key, (FileRef)file.Clone());
                }
            }

            return ret;
        }
    }
}