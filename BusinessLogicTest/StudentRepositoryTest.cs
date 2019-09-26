using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubarashiiDemo.BusinessLogic;
using SubarashiiDemo.Model;

namespace Subarashii.BusinessLogicTest
{
    [TestClass]
    public class StudentRepositoryTest
    {

        [TestMethod]
        public void AddTest()
        {
            StudentRepository repo = new StudentRepository();
            repo.Add(CreateStudent());
            List <Student> l = repo.GetAll();
            Assert.IsTrue(l.Exists(s => s.Id == "123456"));
        }

        [TestMethod]
        public void DeleteTest() {
            StudentRepository repo = new StudentRepository();
            repo.Add(CreateStudent());
            repo.Delete(CreateStudent());
            List<Student> l = repo.GetAll();
            Assert.AreEqual(0,l.Count);
        }


        private Student CreateStudent()
        {
            Student student = new Student();
            student.FirstName = "Jonathan";
            student.LastName = "Joestar";
            student.Id = "123456";

            return student;
        }
    }
}
