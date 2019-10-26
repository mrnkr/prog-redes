using Gestion.Model;
using Gestion.Repository;
using Gestion.Services;
using Gestion.Services.Impl;
using System;

namespace Gestion.Srv
{
    public class Context : MarshalByRefObject, IContext
    {
        private static Context Instance { get; set; }

        public IStudentService StudentService { get; }
        public ISubjectService SubjectService { get; }

        public static Context GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Context();
            }

            return Instance;
        }
        
        private Context()
        {
            IRepository<Student> studentRepo = new Repository<Student>();
            IRepository<Subject> subjectRepo = new Repository<Subject>();

            StudentService = new StudentService(subjectRepo, studentRepo);
            SubjectService = new SubjectService(subjectRepo);
        }

        public override object InitializeLifetimeService() { return null; }
    }
}
