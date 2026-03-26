using TaxiService.Entities;

namespace TaxiService.Services.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Generate JWT token for user
        /// </summary>
        string GenerateToken(User user);

        /// <summary>
        /// Validate JWT token
        /// </summary>
        bool ValidateToken(string token);

        /// <summary>
        /// Get user ID from token
        /// </summary>
        int? GetUserIdFromToken(string token);
    }
}
