using TaxiService.DTOs.Requests;
using TaxiService.DTOs.Responses;

namespace TaxiService.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request);
        Task<VehicleResponse> GetVehicleByIdAsync(int vehicleId);
        Task<List<VehicleResponse>> GetAllVehiclesAsync();
        Task<List<VehicleResponse>> GetAvailableVehiclesAsync();

        Task<VehicleResponse> AssignDriverToVehicleAsync(AssignDriverRequest request);
        Task<VehicleResponse> UpdateVehicleAsync(int vehicleId, UpdateVehicleRequest request);
        Task DeleteVehicleAsync(int vehicleId);
    }
}
