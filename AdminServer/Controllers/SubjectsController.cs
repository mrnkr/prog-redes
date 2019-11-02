using AdminServer.ViewModels;
using Gestion.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AdminServer.Controllers
{
    public class SubjectsController : ApiController
    { 
    
        IContext Ctx { get; }

        public SubjectsController()
        {
            Ctx = RemotingClient.GetContext();
        }

        // GET: Students
        public IEnumerable<SubjectViewModel> GetAll()
        {
            return Ctx.SubjectService
                .GetAllSubjects()
                .Select(s => SubjectViewModel.FromEntity(s));
        }
    }
}