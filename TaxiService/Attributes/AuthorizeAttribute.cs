using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace TaxiService.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Get token from Authorization header
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            try
            {
                // Try to read JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Get user ID from token
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    // Token is valid
                    context.HttpContext.Items["UserId"] = userId;
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}