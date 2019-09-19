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
            repo.Add(createStudent());
            List <Student> l = repo.GetAll();
            Assert.IsTrue(l.Exists(s => s.Id == "123456"));
        }


        private Student createStudent()
        {
            Student student = new Student();
            student.FirstName = "Jonathan";
            student.LastName = "Joestar";
            student.Id = "123456";

            return student;
        }
    }
}
