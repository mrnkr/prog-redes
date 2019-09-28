using System;
using System.Collections.Generic;
using Gestion.Model;
using Gestion.Repository;
using Gestion.Services;
using Gestion.Services.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServicesTests
{
    [TestClass]
    public class SubjectServiceTest
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
        [ExpectedException(typeof(InactiveSubjectException))]
        public void ShouldNotAddGradeToAnInactiveSubject()
        {
            SubjectService service = new SubjectService(SubjectRepository.GetInstance()
           , StudentRepository.GetInstance());
            Subject subject = CreateSubject();
            subject.IsActive = false;
            SubjectRepository.GetInstance().Modify(subject);
            service.AssignGradeToStudent("1", "123456", 100);
        }

        [TestMethod]
        [ExpectedException(typeof(NotEnlistedException))]
        public void ShouldNotAddGradeToUnexistingRelation()
        {
            SubjectService service = new SubjectService(SubjectRepository.GetInstance()
          , StudentRepository.GetInstance());
            Student eren = CreateStudent();
            service.AssignGradeToStudent("1", "123456", 100);
        }

        [TestMethod]
        [ExpectedException(typeof(NoFilesInSubjectException))]
        public void ShouldNotAddGradeToUnexistingFiles()
        {
            SubjectService service = new SubjectService(SubjectRepository.GetInstance()
          , StudentRepository.GetInstance());
            Student eren = CreateStudent();
            eren.Id = "1";
            eren.Grades.Add("1", null);
            eren.Files.Add("1", null);
            service.StudentRepo.Add(eren);
            service.AssignGradeToStudent("1", "1", 100);
        }

        [TestMethod]
        public void ShouldGradeCorrectly()
        {
            SubjectService service = new SubjectService(SubjectRepository.GetInstance(),
                StudentRepository.GetInstance());
            Student eren = CreateStudent();
            eren.Id = "1";
            eren.Grades.Add("1", null);
            eren.Files.Add("1", CreateFiles());
            service.StudentRepo.Add(eren);
            service.AssignGradeToStudent("1", "1", 100);
            int? val;
            service.StudentRepo.GetAll().Find(s => s.Id == "1").Grades.TryGetValue("1",out val);
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
