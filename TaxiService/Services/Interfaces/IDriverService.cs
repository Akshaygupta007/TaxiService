using TaxiService.DTOs.Requests;
using TaxiService.DTOs.Responses;

namespace TaxiService.Services.Interfaces
{
    public interface IDriverService
    {
        Task<DriverResponse> CreateDriverAsync(CreateDriverRequest request);

        Task<List<DriverResponse>> GetAvailableDriversAsync();

        Task<DriverResponse> UpdateDriverAsync(int driverId, UpdateDriverRequest request);

        Task<DriverResponse> ToggleAvailabilityAsync(int driverId);

        Task<List<DriverResponse>> GetAllDriversAsync();
        
        Task<DriverResponse> GetDriverByIdAsync(int id);
        
        Task DeleteDriverAsync(int id);
    }
}
