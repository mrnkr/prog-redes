using Gestion.Model;
using Gestion.Model.Exceptions;
using Gestion.Repository;
using Gestion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gestion.Services
{
    public class TeacherService : ITeacherService
    {
        private IRepository<Teacher> TeacherRepo { get; }
        private ILogger Logger { get; }

        public TeacherService(IRepository<Teacher> teacherRepo, ILogger logger)
        {
            TeacherRepo = teacherRepo;
            Logger = logger;
        }

        public void SignupTeacher(Teacher t)
        {
            TeacherRepo.Add(t);
            Logger.Log(EventType.TeacherSignup, $"Registered {t.Id} - {t.LastName}, {t.FirstName}");
        }

        public Teacher Login(string email, string password)
        {
            Teacher ret = TeacherRepo.Find(s => s.Email == email 
            && s.VerifyPassword(password)).SingleOrDefault();
            if (ret == null)
            {
                throw new NonExistentEntityException();
            }
            return ret;
        }

    }
}
