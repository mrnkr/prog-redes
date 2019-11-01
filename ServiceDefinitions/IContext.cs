namespace Gestion.Services
{
    public interface IContext
    {
        IStudentService StudentService { get; }
        ISubjectService SubjectService { get; }
        ITeacherService TeacherService { get; }
    }
}
