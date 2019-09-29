using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gestion.Model;
using Gestion.Repository;
using Gestion.Services;
using Gestion.Services.Exceptions;
using System.Collections.Generic;

namespace Gestion.Tests.Services
{
    [TestClass]
    public class SubjectServiceTest
    {
        private IRepository<Subject> SubjectRepo { get; set; }
        private IRepository<Student> StudentRepo { get; set; }
        private SubjectService SubjectSrv { get; set; }

        [TestInitialize]
        public void BeforeEach()
        {
            Subject subject = CreateSubject();
            SubjectRepo = new SubjectRepository();
            SubjectRepo.Add(subject);

            Student student = CreateStudent();
            StudentRepo = new StudentRepository();
            StudentRepo.Add(student);

            SubjectSrv = new SubjectService(SubjectRepo, StudentRepo);
        }

        [TestMethod]
        [ExpectedException(typeof(InactiveSubjectException))]
        public void ShouldNotAddGradeToAnInactiveSubject()
        {
            Subject subject = CreateSubject();
            subject.IsActive = false;
            SubjectRepo.Modify(subject);
            SubjectSrv.AssignGradeToStudent("1", "123456", 100);
        }

        [TestMethod]
        [ExpectedException(typeof(NotEnrolledException))]
        public void ShouldNotAddGradeToUnexistingRelation()
        {
            SubjectSrv.AssignGradeToStudent("1", "123456", 100);
        }

        [TestMethod]
        [ExpectedException(typeof(NoFilesInSubjectException))]
        public void ShouldNotAddGradeToUnexistingFiles()
        {
            Student eren = CreateStudent();
            eren.Id = "1";
            eren.Grades.Add("1", null);
            eren.Files.Add("1", null);
            StudentRepo.Add(eren);
            SubjectSrv.AssignGradeToStudent("1", "1", 100);
        }

        [TestMethod]
        public void ShouldGradeCorrectly()
        {
            Student eren = CreateStudent();
            eren.Id = "1";
            eren.Grades.Add("1", null);
            eren.Files.Add("1", CreateFiles());
            StudentRepo.Add(eren);
            SubjectSrv.AssignGradeToStudent("1", "1", 100);
            int? val;
            StudentRepo.GetAll().Find(s => s.Id == "1").Grades.TryGetValue("1", out val);
            Assert.AreEqual(100, val);
        }


        private Student CreateStudent()
        {
            Student student = new Student();
            student.FirstName = "Eren";
            student.LastName = "Jaeger";
            student.Id = "123456";
            student.Grades = new Dictionary<string, int?>();
            student.Files = new Dictionary<string, List<FileRef>>();
            return student;
        }

        private Subject CreateSubject()
        {
            var subject = new Subject();
            subject.Id = "1";
            subject.Name = "Mira la cara con la que te mira Conan";
            subject.IsActive = true;
            return subject;
        }

        private List<FileRef> CreateFiles()
        {
            FileRef file = new FileRef();
            file.Id = "1";
            file.Name = "Eh Stan Lee";
            file.Path = "C://Users/Aaa/Desktop/StanLee.jpg";
            List<FileRef> files = new List<FileRef>();
            files.Add(file);
            return files;
        }
    }
}
