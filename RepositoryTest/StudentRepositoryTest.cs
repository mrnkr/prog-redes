using Gestion.Model;
using Gestion.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Gestion.Tests.Repository
{
    [TestClass]
    public class StudentRepositoryTest
    {

        [TestCleanup]
        public void AfterEach() {
            StudentRepository.GetInstance().GetAll().Clear();
        }

        [TestMethod]
        public void ShouldAddStudent()
        {
            StudentRepository repo = StudentRepository.GetInstance();
            repo.Add(CreateStudent());
            List <Student> l = repo.GetAll();
            Assert.IsTrue(l.Exists(s => s.Id == "123456"));
        }

        [TestMethod]
        public void ShouldDeleteStudent() {

            Student coso = CreateStudent();
            StudentRepository repo = StudentRepository.GetInstance();
            repo.Add(coso);
            repo.Delete(coso);
            List<Student> l = repo.GetAll();
            Assert.AreEqual(0,l.Count);
        }

        [TestMethod]
        public void ShouldModifyStudent()
        {
            Student coso = CreateStudent();
            StudentRepository repo = StudentRepository.GetInstance();
            repo.Add(coso);
            coso.Grades.Add("1", 100);
            repo.Modify(coso);
            int? val = 0; ;
            repo.GetAll().Find(s => s.Id == coso.Id).Grades.TryGetValue("1", out val);
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
