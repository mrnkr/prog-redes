using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gestion.Common
{
    public class JwtHelpers
    {
        private string SecretKey { get; set; }
        private string Issuer { get; set; }
        private int ExpiryInMinutes { get; set; }

        public JwtHelpers(string secretKey, string issuer, int expiryInMinutes)
        {
            SecretKey = secretKey;
            Issuer = issuer;
            ExpiryInMinutes = expiryInMinutes;
        }

        public string CreateTokenWithUid(string uid)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, uid)
            });

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = tokenHandler.CreateJwtSecurityToken(
                    issuer: Issuer,
                    audience: "access",
                    subject: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(ExpiryInMinutes),
                    signingCredentials: signingCredentials);

            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(SecretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = "access",
                ValidIssuer = Issuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                LifetimeValidator = LifetimeValidator,
                IssuerSigningKey = securityKey
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                throw new SecurityTokenDecryptionFailedException();
            }
        }

        private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }

            return false;
        }
    }
}
