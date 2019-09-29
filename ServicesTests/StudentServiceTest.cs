using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gestion.Model;
using Gestion.Model.Exceptions;
using Gestion.Repository;
using Gestion.Services;
using Gestion.Services.Exceptions;
using System.Collections.Generic;

namespace Gestion.Tests.Services
{
    [TestClass]
    public class StudentServiceTest
    {
        private IRepository<Subject> SubjectRepo { get; set; }
        private IRepository<Student> StudentRepo { get; set; }
        private StudentService StudentSrv { get; set; }

        [TestInitialize]
        public void BeforeEach()
        {
            Subject subject = CreateSubject();
            SubjectRepo = new SubjectRepository();
            SubjectRepo.Add(subject);

            Student student = CreateStudent();
            StudentRepo = new StudentRepository();
            StudentRepo.Add(student);

            StudentSrv = new StudentService(SubjectRepo, StudentRepo);
        }

        [TestMethod]
        public void ShouldEnrollCorrectly()
        {
            StudentSrv.EnrollInSubject("123456", "1");
            Assert.IsTrue(StudentRepo.GetAll().Find(s => s.Id == "123456").Grades.ContainsKey("1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InactiveSubjectException))]
        public void ShouldThrowInactiveException()
        {
            Subject inactive = CreateSubject();
            inactive.Id = "2";
            inactive.IsActive = false;
            SubjectRepo.Add(inactive);
            StudentSrv.EnrollInSubject("123456", "2");
        }

        [TestMethod]
        [ExpectedException(typeof(UndefinedSubjectException))]
        public void ShouldThrowUndefinedException()
        {
            StudentSrv.AddFileToSubject(CreateFile(), "1", "123456");
        }

        [TestMethod]
        public void ShouldAddFileCorrectly()
        {
            StudentSrv.EnrollInSubject("123456", "1");
            StudentSrv.AddFileToSubject(CreateFile(), "1", "123456");
        }
        private Student CreateStudent()
        {
            Student student = new Student
            {
                FirstName = "Eren",
                LastName = "Jaeger",
                Id = "123456",
                Grades = new Dictionary<string, int?>(),
                Files = new Dictionary<string, List<FileRef>>()
            };
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

        private FileRef CreateFile()
        {
            FileRef file = new FileRef
            {
                Id = "1",
                Name = "Eh Stan Lee",
                Path = "C://Users/Aaa/Desktop/StanLee.jpg"
            };
            return file;
        }
    }
}
