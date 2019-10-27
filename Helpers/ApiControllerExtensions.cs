using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Gestion.Common
{
    public static class ApiControllerExtensions
    {
        public static string GetLoggedUserId(this ApiController ctrl)
        {
            return ((ClaimsIdentity)ctrl.User.Identity)
                .Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .SingleOrDefault();
        }
    }
}