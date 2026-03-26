using TaxiService.Entities;

namespace TaxiService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByEmailAsync(string email);
        Task<IList<User>> GetAllAsync();
        Task AddAsync(User user);
        Task SaveChangesAsync();
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}
