using Gestion.Model;
using Subarashii.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gestion.Srv
{
    public class ValuesController : Controller
    {
        [Handler("01")]
        public void VerifyStudentExists(string studentId, string auth)
        {
            var studentService = Context.GetInstance().StudentService;
            var studentExists = studentService.StudentDoesExist(studentId);
            
            if (studentExists)
            {
                Text("HELO");
            }
            else
            {
                Text("BYE!");
            }
        }

        [Handler("02")]
        public void GetStudentsName(string studentId, string auth)
        {
            var studentService = Context.GetInstance().StudentService;
            var student = studentService.GetStudentById(studentId);
            Text(student.FirstName);
        }

        [Handler("03")]
        public void GetSubjects(string scope, string auth)
        {
            var subjectService = Context.GetInstance().SubjectService;
            var studentService = Context.GetInstance().StudentService;

            switch (scope)
            {
                case "ALL":
                    Object(subjectService.GetAllSubjects());
                    break;
                case "MINE":
                    Object(studentService.GetSubjectsStudentIsEnrolledIn(auth));
                    break;
                case "ALL_WITH_STATUS":
                    Object(studentService.GetSubjectsAndStatusAccordingToStudent(auth));
                    break;
                case "NOT_MINE":
                    var subjectsUserIsEnrolledIn = studentService.GetSubjectsStudentIsEnrolledIn(auth).Select(s => s.Id);
                    Object(subjectService.GetAllSubjects().Where(s => !subjectsUserIsEnrolledIn.Contains(s.Id)));
                    break;
                default:
                    Object(new { Error = "Wrong scope" });
                    break;
            }
        }

        [Handler("04")]
        public void EnrollInSubjectById(string subjectId, string auth)
        {
            try
            {
                var studentService = Context.GetInstance().StudentService;
                studentService.EnrollInSubject(auth, subjectId);
                Text("OK");
            }
            catch
            {
                Text("ERROR");
            }
        }

        [Handler("05")]
        public void UploadFile(string fileName, string auth)
        {
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var src = Path.Combine(userFolder, "Downloads", fileName);

            if (auth == "------")
            {
                System.IO.File.Delete(src);
                Object(new { Error = "Not authenticated" });
                return;
            }

            var destDir = Path.Combine(userFolder, "Downloads", auth);
            var dest = Path.Combine(destDir, fileName);

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            System.IO.File.Move(src, dest);
            Object(new FileRef()
            {
                Name = fileName,
                Path = dest
            });
        }

        [Handler("06")]
        public void LinkUploadedFileToSubject(Tuple<string, FileRef> data, string auth)
        {
            try
            {
                var studentService = Context.GetInstance().StudentService;
                var student = studentService.GetStudentById(auth);
                student.AddFileToSubject(data.Item1, data.Item2);
                Text("OK");
            }
            catch
            {
                System.IO.File.Delete(data.Item2.Path);
                Text("ERROR");
            }
        }

        [Handler("07")]
        public void GetGradesForStudent(string scope, string auth)
        {
            IDictionary<string, int> ret = new Dictionary<string, int>();
            var studentService = Context.GetInstance().StudentService;
            var subjectService = Context.GetInstance().SubjectService;

            var student = studentService.GetStudentById(auth);
            var subjects = student.GetGrades();

            foreach (var s in subjects)
            {
                var subject = subjectService.GetSubjectById(s.Key);
                ret.Add(subject.Name, s.Value);
            }

            Object(ret);
        }
    }
}
