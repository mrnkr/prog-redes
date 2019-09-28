using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gestion.Services.Exceptions;
using Gestion.Services;
using Gestion.Model;
using Gestion.Repository;
using Gestion.Model.Exceptions;

namespace ServicesTests
{
    [TestClass]
    public class StudentServiceTest
    {

        [TestInitialize]
        public void BeforeEach()
        {
            Student student = CreateStudent();
            Subject subject = CreateSubject();
            SubjectRepository.GetInstance().Add(subject);
            StudentRepository.GetInstance().Add(student);
        }
        [TestCleanup]
        public void AfterEach()
        {
            SubjectRepository.GetInstance().GetAll().Clear();
            StudentRepository.GetInstance().GetAll().Clear();
        }


        [TestMethod]
        public void ShouldEnrollCorrectly()
        {
            StudentService service = new StudentService(SubjectRepository.GetInstance(),
                StudentRepository.GetInstance());
            service.EnrollInSubject("123456", "1");
            Assert.IsTrue(service.StudentRepo.GetAll().
                Find(s => s.Id == "123456").Grades.ContainsKey("1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InactiveSubjectException))]
        public void ShouldThrowInactiveException()
        {
            StudentService service = new StudentService(SubjectRepository.GetInstance(),
               StudentRepository.GetInstance());
            Subject inactive = CreateSubject();
            inactive.Id = "2";
            inactive.IsActive = false;
            service.SubjectRepo.Add(inactive);
            service.EnrollInSubject("123456", "2");
        }

        [TestMethod]
        [ExpectedException(typeof(UndefinedSubjectException))]
        public void ShouldThrowUndefinedException()
        {
            StudentService service = new StudentService(SubjectRepository.GetInstance(),
             StudentRepository.GetInstance());
            service.AddFileToSubject(CreateFile(), "1", "123456");
        }

        [TestMethod]
        public void ShouldAddFileCorrectly()
        {
            StudentService service = new StudentService(SubjectRepository.GetInstance(),
             StudentRepository.GetInstance());
            service.EnrollInSubject("123456", "1");
            service.AddFileToSubject(CreateFile(), "1", "123456");
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
