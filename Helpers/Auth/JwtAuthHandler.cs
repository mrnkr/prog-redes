using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Gestion.Common
{
    public class JwtAuthHandler : DelegatingHandler
    {
        private JwtHelpers Jwt { get; }

        public JwtAuthHandler(string secretKey, string issuer, int expiryInMinutes)
        {
            Jwt = new JwtHelpers(secretKey, issuer, expiryInMinutes);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var headers = request.Headers;
                var token = headers.Authorization.Parameter;

                var principal = Jwt.DecodeToken(token);
                Thread.CurrentPrincipal = principal;
                HttpContext.Current.User = principal;
            }
            catch (SecurityTokenDecryptionFailedException)
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { }, cancellationToken);
            }
            catch
            {
                // Let anonymous user through, validation will happen elsewhere if needed
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
