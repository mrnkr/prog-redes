using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion.Model
{
    public class Teacher : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set;  }

        public override object Clone()
        {
            return new Teacher
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                Id = Id,
            };
        }
    }
}
