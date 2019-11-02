using AdminServer.Exceptions;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace AdminServer.App_Start
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is UnauthorizedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            else
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
