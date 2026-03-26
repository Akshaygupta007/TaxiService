using TaxiService.Entities;

namespace TaxiService.Repositories.Interfaces
{
    public interface IDriverRepository
    {
        Task<Driver?> GetByIdAsync(int id);
        Task<IEnumerable<Driver>> GetAvailableDriversAsync();
        Task<IEnumerable<Driver>> GetAllAsync();
        Task AddAsync(Driver driver);
        Task UpdateAsync(Driver driver);

        Task DeleteAsync(Driver driver);
    }
}
