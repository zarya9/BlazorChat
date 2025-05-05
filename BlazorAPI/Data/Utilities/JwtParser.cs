using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorAPI.Data.Utilities
{
    public static class JwtParser
    {
        public static string GetRoleFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Стандартное claim для роли
                var roleClaim = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Role ||
                                       c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

                return roleClaim?.Value;
            }
            catch
            {
                return null;
            }
        }

        public static string GetEmailFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var emailClaim = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email ||
                                       c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

                return emailClaim?.Value;
            }
            catch
            {
                return null;
            }
        }

        public static bool IsTokenValid(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                return jwtToken.ValidTo > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }
    }
}
