using Gestion.Common;
using AdminServer.ViewModels;
using System.Web.Http;
using Gestion.Services;
using AdminServer.Exceptions;

namespace AdminServer.Controllers
{
    public class LoginController : ApiController
    {
        IContext Ctx { get; }

        public LoginController()
        {
            Ctx = RemotingClient.GetContext();
        }

        public TokenResponseViewModel Login([FromBody] LoginViewModel data)
        {
            try
            {
                var teacher = Ctx.TeacherService.Login(data.username, data.password);

                var jwt = new JwtHelpers(
                    secretKey: Config.GetValue<string>("Jwt:SigningKey"),
                    issuer: Config.GetValue<string>("Jwt:Site"),
                    expiryInMinutes: Config.GetValue<int>("Jwt:ExpiryInMinutes"));

                return TokenResponseViewModel.WithToken(jwt.CreateTokenWithUid(teacher.Id));
            }
            catch
            {
                throw new UnauthorizedException();
            }
        }
    }
}