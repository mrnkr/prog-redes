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
        public void VerifyStudent(string studentId, string auth)
        {
            try
            {
                var studentService = Context.GetInstance().StudentService;
                var student = studentService.GetStudentById(studentId);
                Text(student.FirstName);
            }
            catch
            {
                Error();
            }
        }

        [Handler("03")]
        public void GetSubjects(string scope, string auth)
        {
            try
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
                        throw new Exception();
                }
            }
            catch
            {
                Error();
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
                Error();
            }
        }

        [Handler("05")]
        public void UploadFile(string fileName, string auth)
        {
            try
            {
                string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var src = Path.Combine(userFolder, "Downloads", fileName);

                if (auth == "------")
                {
                    System.IO.File.Delete(src);
                    throw new Exception();
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
            catch
            {
                Error();
            }
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
            try
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
            catch
            {
                Error();
            }
        }
    }
}
