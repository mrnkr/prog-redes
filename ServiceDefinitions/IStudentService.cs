using Gestion.Model;
using System.Collections.Generic;

namespace Gestion.Services
{
    public interface IStudentService
    {
        Student GetStudentById(string studentId);
        IEnumerable<Student> GetStudentsEnrolledInSubject(string subjectId);
        void SignupStudent(Student s);
        IEnumerable<Subject> GetSubjectsStudentIsEnrolledIn(string studentId);
        IDictionary<string, string> GetSubjectsAndStatusAccordingToStudent(string studentId);
        void EnrollInSubject(string studentId, string subjectId);
        void LinkUploadedFileToSubjectForStudent(string studentId, string subjectId, FileRef file);
        IEnumerable<FileRef> GetFilesUploadedByStudent(string studentId, string subjectId);
        void GradeStudent(string studentId, string subjectId, int grade);
    }
}
