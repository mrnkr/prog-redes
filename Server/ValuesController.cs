using Gestion.Model;
using Gestion.Services;
using Subarashii.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gestion.Srv
{
    public class ValuesController : Controller
    {
        private IStudentService StudentSrv { get; }
        private ISubjectService SubjectSrv { get; }

        public ValuesController()
        {
            StudentSrv = Context.GetInstance().StudentService;
            SubjectSrv = Context.GetInstance().SubjectService;
        }

        [Handler("01")]
        public void VerifyStudent(string studentId, string auth)
        {
            try
            {
                var student = StudentSrv.GetStudentById(studentId);
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
                switch (scope)
                {
                    case "ALL":
                        Object(SubjectSrv.GetAllSubjects());
                        break;
                    case "MINE":
                        Object(StudentSrv.GetSubjectsStudentIsEnrolledIn(auth));
                        break;
                    case "ALL_WITH_STATUS":
                        Object(StudentSrv.GetSubjectsAndStatusAccordingToStudent(auth));
                        break;
                    case "NOT_MINE":
                        var subjectsUserIsEnrolledIn = StudentSrv.GetSubjectsStudentIsEnrolledIn(auth).Select(s => s.Id);
                        Object(SubjectSrv.GetAllSubjects().Where(s => !subjectsUserIsEnrolledIn.Contains(s.Id)));
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
                StudentSrv.EnrollInSubject(auth, subjectId);
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
            string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var src = Path.Combine(userFolder, "Downloads", fileName);

            try
            {
                if (auth == "------")
                {
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
                System.IO.File.Delete(src);
                Error();
            }
        }

        [Handler("06")]
        public void LinkUploadedFileToSubject(Tuple<string, FileRef> data, string auth)
        {
            try
            {
                StudentSrv.LinkUploadedFileToSubjectForStudent(auth, data.Item1, data.Item2);
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

                var student = StudentSrv.GetStudentById(auth);
                var grades = student.GetGrades();

                foreach (var s in grades)
                {
                    var subject = student.GetSubjects().Where(sub => sub.Id == s.Key).Single();
                    ret.Add(subject.Name, s.Value);
                }

                Object(ret);
            }
            catch
            {
                Error();
            }
        }

        [Handler("08")]
        public void GetFilesForStudent(string subjectId, string auth)
        {
            try
            {
                var files = StudentSrv.GetFilesUploadedByStudent(auth, subjectId);
                Object(files);
            }
            catch
            {
                Error();
            }
        }

        [Handler("09")]
        public void DownloadFile(string path, string auth)
        {
            try
            {
                File(path);
            }
            catch
            {
                Error();
            }
        }
    }
}
