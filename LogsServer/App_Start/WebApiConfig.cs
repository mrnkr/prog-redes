using Gestion.Common;
using LogsServer.App_Start;
using System.Web.Http;

namespace LogsServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new CustomExceptionFilterAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new JwtAuthHandler(
                secretKey: Config.GetValue<string>("Jwt:SigningKey"),
                issuer: Config.GetValue<string>("Jwt:Site"),
                expiryInMinutes: Config.GetValue<int>("Jwt:ExpiryInMinutes")));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
