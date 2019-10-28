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
        [Authorize]
        public IEnumerable<SubjectViewModel> Get()
        {
            // To know the id of the user that's logged in
            // import Gestion.Common and use the extension method
            //var uid = this.GetLoggedUserId();

            return Ctx.SubjectService
                .GetAllSubjects()
                .Select(s => SubjectViewModel.FromEntity(s));
        }
    }
}
