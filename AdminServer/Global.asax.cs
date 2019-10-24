using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AdminServer
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RemotingClient.Setup();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
