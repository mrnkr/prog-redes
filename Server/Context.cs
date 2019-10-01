using Gestion.Model;
using Gestion.Repository;
using Gestion.Services;

namespace Gestion.Srv
{
    public class Context
    {
        private static Context Instance { get; set; }

        public StudentService StudentService { get; }
        public SubjectService SubjectService { get; }

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
    }
}
