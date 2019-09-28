using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subarashii.Repository;
using SubarashiiDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subarashii.RepositoryTests
{
    [TestClass]
    public class SubjectRepositoryTest
    {

        [TestCleanup]
        public void AfterEach()
        {
            SubjectRepository.GetInstance().GetAll().Clear();
        }

        [TestMethod]
        public void ShouldAddSubject()
        {
            SubjectRepository repo = SubjectRepository.GetInstance();
            repo.Add(CreateSubject());
            List<Subject> l = repo.GetAll();
            Assert.IsTrue(l.Exists(s => s.Id == "1"));
        }

        [TestMethod]
        public void ShouldDeleteSubject()
        {
            SubjectRepository repo = SubjectRepository.GetInstance();
            repo.Add(CreateSubject());
            repo.Delete(CreateSubject());
            List<Subject> l = repo.GetAll();
            Assert.AreEqual(1, l.Count);
            Assert.AreEqual(false, repo.GetAll().Find(s => s.Id == "1").IsActive);
        }
        [TestMethod]
        public void ShouldModifySubject()
        {
            Subject s = CreateSubject();
            SubjectRepository repo = SubjectRepository.GetInstance();
            repo.Add(s);
            s.Name = "Astrometafilofisica mechacuantica";
            repo.Modify(s);
            Assert.AreEqual("Astrometafilofisica mechacuantica", 
                repo.GetAll().Find(sa => sa.Id == s.Id).Name);
        }


        private Subject CreateSubject()
        {
            var subject = new Subject();
            subject.Id = "1";
            subject.Name = "Astrometafisica cuantica";
            subject.IsActive = true;
            return subject;
        }
    }
}
