using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gestion.Model;
using Gestion.Repository;
using System.Collections.Generic;

namespace Gestion.Tests.Repository
{
    [TestClass]
    public class SubjectRepositoryTest
    {
        private IRepository<Subject> SubjectRepo { get; set; }

        [TestInitialize]
        public void BeforeEach()
        {
            SubjectRepo = new SubjectRepository();
        }

        [TestMethod]
        public void ShouldAddSubject()
        {
            SubjectRepo.Add(CreateSubject());
            List<Subject> l = SubjectRepo.GetAll();
            Assert.IsTrue(l.Exists(s => s.Id == "1"));
        }

        [TestMethod]
        public void ShouldDeleteSubject()
        {
            SubjectRepo.Add(CreateSubject());
            SubjectRepo.Delete(CreateSubject());
            List<Subject> l = SubjectRepo.GetAll();
            Assert.AreEqual(1, l.Count);
            Assert.AreEqual(false, SubjectRepo.GetAll().Find(s => s.Id == "1").IsActive);
        }
        [TestMethod]
        public void ShouldModifySubject()
        {
            Subject s = CreateSubject();
            SubjectRepo.Add(s);
            s.Name = "Astrometafilofisica mechacuantica";
            SubjectRepo.Modify(s);
            Assert.AreEqual("Astrometafilofisica mechacuantica",
                SubjectRepo.GetAll().Find(sa => sa.Id == s.Id).Name);
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
