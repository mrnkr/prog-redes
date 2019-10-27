using Gestion.Common;
using AdminServer.ViewModels;
using System.Web.Http;

namespace AdminServer.Controllers
{
    public class LoginController : ApiController
    {
        public TokenResponseViewModel Login([FromBody] LoginViewModel data)
        {
            var jwt = new JwtHelpers(
                secretKey: Config.GetValue<string>("Jwt:SigningKey"),
                issuer: Config.GetValue<string>("Jwt:Site"),
                expiryInMinutes: Config.GetValue<int>("Jwt:ExpiryInMinutes"));

            return TokenResponseViewModel.WithToken(jwt.CreateTokenWithUid(data.uid));
        }
    }
}