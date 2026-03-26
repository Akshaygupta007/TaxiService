using TaxiService.Entities;

namespace TaxiService.Repositories.Interfaces
{
    public interface ICabTypeRepository
    {
        Task<CabType?> GetByIdAsync(int id);
        Task<IList<CabType>> GetAllAsync();
        Task<CabType> AddAsync(CabType cabType);

        Task SaveChangesAsync();
        Task <CabType?> CabTypeNameExistsAsync(string cabTypeName);

        Task UpdateAsync(CabType cabType);

        Task DeleteAsync(CabType cabType);

    }
}
