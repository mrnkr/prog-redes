using System;

namespace Gestion.Model
{
    public class Teacher : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Teacher()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void EncryptPassword()
        {
            Password = BCrypt.Net.BCrypt.HashPassword(Password);
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }

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
