using LogsServer.Exceptions;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace LogsServer.App_Start
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is InvalidEventTypeException || context.Exception is InvalidSorterException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
