using Gestion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AdminServer.ViewModels;

namespace AdminServer.Controllers
{
    public class SignupController : ApiController
    {
        IContext Ctx { get; }

        public SignupController()
        {
            Ctx = RemotingClient.GetContext();
        }

        //Post: api/signup
        [Authorize]
        public string SignUpTeacher([FromBody] SignupViewModel model)
        {
            Ctx.TeacherService.SignupTeacher(model.ToEntity());
            return "Teacher created";
        }
    }
}