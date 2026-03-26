using Microsoft.EntityFrameworkCore;
using TaxiService.DataDb;
using TaxiService.Entities;
using TaxiService.Repositories.Interfaces;


namespace TaxiService.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.Trim().ToLower());
        }

        public async Task<IList<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(User user)
        {

            _context.Users.Remove(user);
            await Task.CompletedTask;

        }
    }
}
