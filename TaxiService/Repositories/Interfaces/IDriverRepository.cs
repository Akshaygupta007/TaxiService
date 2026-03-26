using TaxiService.DTOs.Responses;
using TaxiService.Entities;

namespace TaxiService.Repositories.Interfaces
{
    public interface IDriverRepository
    {
        Task<Driver?> GetByIdAsync(int id);
        Task<IList<Driver>> GetAvailableDriversAsync();
        Task<IList<Driver>> GetAllAsync();
        Task<Driver> AddAsync(Driver driver);

        Task<Driver?> GetByLicenseAsync(string licenseNumber);
        Task<Driver?> GetByPhoneNumberAsync(string phoneNumber);
        Task SaveChangesAsync();
        Task UpdateAsync(Driver driver);

        Task DeleteAsync(Driver driver);
    }
}
