using TaxiService.DTOs.Requests;
using TaxiService.DTOs.Responses;
using TaxiService.Entities;

namespace TaxiService.Services.Interfaces
{
    public interface ICabTypeService
    {

        Task<CabTypeResponse> CreateCabTypeAsync(CreateCabTypeRequest request);
        Task<List<CabTypeResponse>> GetAllCabTypesAsync();
        Task<CabTypeResponse> GetCabTypeIdAsync(int id);
        Task<CabTypeResponse> UpdateCabType(int id, UpdateCabTypeRequest request);

        Task DeleteUserAsync(int id);
    }
}
