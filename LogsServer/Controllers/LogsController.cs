using LogsServer.Logger;
using LogsServer.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LogsServer.Controllers
{
    public class LogsController : ApiController
    {
        [Authorize]
        public IEnumerable<LogEntryViewModel> Get([FromUri] LogQueryViewModel logQuery)
        {
            var query = LogsService
                .GetInstance()
                .QueryLogs();
            logQuery.PrepareQuery(query);

            return query
                .RunQuery()
                .Select(l => LogEntryViewModel.FromEntity(l));
        }
    }
}
