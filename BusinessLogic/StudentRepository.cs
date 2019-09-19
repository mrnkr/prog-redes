﻿using System;
using System.Collections.Generic;
using System.Text;
using Model.Exceptions;
using SubarashiiDemo.Model;

namespace SubarashiiDemo.BusinessLogic
{
    public class StudentRepository : IRepository<Student>
    {
        private List<Student> Students;
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
            Student stud = Students.Find(p => p.Id == obj.Id);
            Students.Remove(stud);
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
