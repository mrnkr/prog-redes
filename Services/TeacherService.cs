using Gestion.Model;
using Gestion.Repository;
using Gestion.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gestion.Services
{
    public class TeacherService : ITeacherService
    {
        private IRepository<Teacher> TeacherRepo { get; }
        private IRepository<Student> StudentRepo { get; }
        private IRepository<Subject> SubjectRepo { get; }
        private ILogger Logger { get; }

        public TeacherService(IRepository<Teacher> teacherRepo, IRepository<Subject> subjectRepo,
            IRepository<Student> studentRepo, ILogger logger)
        {
            TeacherRepo = teacherRepo;
            SubjectRepo = subjectRepo;
            StudentRepo = studentRepo;
            Logger = logger;
        }

        public void SignupTeacher(Teacher t)
        {
            TeacherRepo.Add(t);

            Logger.Log(EventType.TeacherSignup, $"Registered {t.Id} - {t.LastName}, {t.FirstName}");
        }

        public void MarkUnmarkedMaterial(string teachedId, string studentId, 
            string subjectId, int grade)
        {
            Student student = StudentRepo.Get(studentId);
            student.AddGrade(subjectId, grade);
            StudentRepo.Update(student);
            Subject subject = SubjectRepo.Get(subjectId);

            Logger.Log(EventType.Grading, $"Graded {student.FirstName} , {student.LastName}," +
                $" in Subject {subject.Name} with grade {grade}");
        }

       
    }
}
