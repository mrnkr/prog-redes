using AdminServer.ViewModels;
using Gestion.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AdminServer.Controllers
{
    public class ValuesController : ApiController
    {
        IContext Ctx { get; }

        public ValuesController()
        {
            Ctx = RemotingClient.GetContext();
        }

        // GET api/values
        public IEnumerable<SubjectViewModel> Get()
        {
            return Ctx.SubjectService
                .GetAllSubjects()
                .Select(s => SubjectViewModel.FromEntity(s));
        }
    }
}
