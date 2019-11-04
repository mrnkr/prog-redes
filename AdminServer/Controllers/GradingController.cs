using AdminServer.ViewModels;
using Gestion.Services;
using System.Web.Http;

namespace AdminServer.Controllers
{
    public class GradingController : ApiController
    {
        IContext Ctx { get; }

        public GradingController()
        {
            Ctx = RemotingClient.GetContext();
        }

        //POST: api/grading
        [Authorize]
        public string GradeStudent([FromBody] GradingViewModel model)
        {
            Ctx.StudentService.GradeStudent(model.studentId, model.subjectId, model.grade);
            return "Student has been graded succesfully";
        }
    }
}
