using System;
using System.Collections.Generic;
using System.Text;
using Model.Exceptions;
using SubarashiiDemo.Model;

namespace Subarashii.Repository
{
    public class StudentRepository : IRepository<Student>
    {
        private List<Student> Students { get; set; }
        private static StudentRepository Instance { get; set; }

        private StudentRepository()
        {
            Students = new List<Student>();
        }

        public static StudentRepository GetInstance()
        {
            if (Instance == null)
            {
                Instance = new StudentRepository();
            }
            return Instance;
        }

        public void Add(Student objectToCreate)
        {
            Students.Add(objectToCreate);
        }

        public void Delete(Student objectToDelete)
        {
            if (!Exists(objectToDelete.Id))
            {
                throw new NonExistentStudentException();
            }
            Students.Remove(objectToDelete);
        }

        public void Modify(Student obj)
        {
            if (!Exists(obj.Id))
            {
                throw new NonExistentStudentException();
            }
            Student student= Students.Find(p => p.Id == obj.Id);
            Students.Remove(student);
            Students.Add(obj);
        }

        public List<Student> GetAll()
        {
            return Students;
        }

        private bool Exists(string id)
        {
            return Students.Exists(s => s.Id == id);
        }
    }
}
