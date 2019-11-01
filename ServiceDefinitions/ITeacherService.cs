using Gestion.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion.Services
{
    public interface ITeacherService
    {
        void SignupTeacher(Teacher t);

        Teacher Login(string email, string password);
    }
}
