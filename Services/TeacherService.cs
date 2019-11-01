using Gestion.Model;
using Gestion.Model.Exceptions;
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

        public Teacher Login(string email, string password)
        {
            IEnumerable<Teacher> teachers = TeacherRepo.Find(s => s.Email == email);
            Teacher ret = null;
            foreach (var v in teachers)
            {
                bool passes = v.VerifyPassword(password);
                if (passes)
                {
                    ret = v;
                }
            }
            if (ret == null)
            {
                throw new NonExistentEntityException();
            }
            return ret;
        }

    }
}
