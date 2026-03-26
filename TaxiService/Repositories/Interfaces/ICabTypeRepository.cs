using TaxiService.Entities;

namespace TaxiService.Repositories.Interfaces
{
    public interface ICabTypeRepository
    {
        Task<CabType?> GetByIdAsync(int id);
        Task<IEnumerable<CabType>> GetAllAsync();
        Task AddAsync(CabType cabType);
        Task UpdateAsync(CabType cabType);

        Task DeleteAsync(CabType cabType);
    }
}
