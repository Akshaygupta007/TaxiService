using TaxiService.Common;
using TaxiService.DTOs.Requests;
using TaxiService.DTOs.Responses;

namespace TaxiService.Services.Interfaces
{
    public interface IUserService
    {

        Task<UserResponse> RegisterAsync(RegisterRequest request);
        Task<UserResponse> LoginAsync(LoginRequest request);
        Task<List<UserResponse>> GetAllUsersAsync();
        Task<UserResponse> GetUserByIdAsync(int id);
        Task DeleteUserAsync(int id);
    }
}
