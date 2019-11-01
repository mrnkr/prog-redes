using Gestion.Model;
using Gestion.Repository;
using Gestion.Services;
using Gestion.Services.Impl;
using Gestion.Srv.Logger;
using System;

namespace Gestion.Srv
{
    public class Context : MarshalByRefObject, IContext
    {
        private static Context Instance { get; set; }

        public IStudentService StudentService { get; }
        public ISubjectService SubjectService { get; }
        
        public ITeacherService TeacherService { get; }

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
            IRepository<Teacher> teacherRepo = new Repository<Teacher>();
            ILogger logger = new MessageQueueLogger();

            StudentService = new StudentService(subjectRepo, studentRepo, logger);
            SubjectService = new SubjectService(subjectRepo, logger);
            TeacherService = new TeacherService(teacherRepo, logger);
        }

        public override object InitializeLifetimeService() { return null; }
    }
}
