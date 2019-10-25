using LogsServer.Logger;
using LogsServer.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LogsServer.Controllers
{
    public class ValuesController : ApiController
    {
        public IEnumerable<LogEntryViewModel> Get()
        {
            return LogsService
                .GetInstance()
                .GetAll()
                .Select(l => LogEntryViewModel.FromEntity(l));
        }
    }
}
