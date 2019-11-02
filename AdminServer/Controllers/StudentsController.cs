using AdminServer.ViewModels;
using Gestion.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AdminServer.Controllers
{
    public class StudentsController : ApiController
    {
        IContext Ctx { get; }

        public StudentsController()
        {
            Ctx = RemotingClient.GetContext();
        }

        // GET: Students
        public IEnumerable<StudentViewModel> GetAll([FromUri] StudentQueryViewModel query)
        {
            return Ctx.StudentService
                .GetStudentsEnrolledInSubject(query.subjectId)
                .Select(s => StudentViewModel.FromEntity(s));
        }
    }
}