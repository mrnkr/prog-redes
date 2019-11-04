using AdminServer.Exceptions;
using Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminServer.ViewModels
{
    public class SignupViewModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public Teacher ToEntity()
        {
            AssertNoneEmpty();
            return new Teacher
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
            };
        }
        
        private void AssertNoneEmpty()
        {
            var t = typeof(SignupViewModel);
            var anyEmpty = t
                .GetProperties()
                .Any(p => ((string)p.GetValue(this)).Length == 0);

            if (anyEmpty)
            {
                throw new EmptyStringException();
            }
        }
    }
}