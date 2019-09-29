using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gestion.Model;
using Gestion.Repository;
using System.Collections.Generic;

namespace Gestion.Tests.Repository
{
    [TestClass]
    public class StudentRepositoryTest
    {
        private IRepository<Student> StudentRepo { get; set; }

        [TestInitialize]
        public void BeforeEach()
        {
            StudentRepo = new StudentRepository();
        }

        [TestMethod]
        public void ShouldAddStudent()
        {
            StudentRepo.Add(CreateStudent());
            List <Student> l = StudentRepo.GetAll();
            Assert.IsTrue(l.Exists(s => s.Id == "123456"));
        }

        [TestMethod]
        public void ShouldDeleteStudent() {

            Student coso = CreateStudent();
            StudentRepo.Add(coso);
            StudentRepo.Delete(coso);
            List<Student> l = StudentRepo.GetAll();
            Assert.AreEqual(0, l.Count);
        }

        [TestMethod]
        public void ShouldModifyStudent()
        {
            Student coso = CreateStudent();
            StudentRepo.Add(coso);
            coso.Grades.Add("1", 100);
            StudentRepo.Modify(coso);
            int? val = 0; ;
            StudentRepo.GetAll().Find(s => s.Id == coso.Id).Grades.TryGetValue("1", out val);
            Assert.AreEqual(100, val);
        }

        private Student CreateStudent()
        {
            Student student = new Student();
            student.FirstName = "Jonathan";
            student.LastName = "Joestar";
            student.Id = "123456";
            student.Grades = new Dictionary<string, int?>();
            student.Files = new Dictionary<string, List<FileRef>>();
            return student;
        }
    }
}
