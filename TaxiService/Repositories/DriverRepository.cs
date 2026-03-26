using Microsoft.EntityFrameworkCore;
using TaxiService.DataDb;
using TaxiService.Entities;
using TaxiService.Repositories.Interfaces;

namespace TaxiService.Repositories
{
    public class DriverRepository : IDriverRepository
    {

        private readonly ApplicationDbContext _context;

        public DriverRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Driver?> GetByIdAsync(int id)
        {
            return await _context.Drivers.FindAsync(id);
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task AddAsync(Driver Driver)
        {
            await _context.Drivers.AddAsync(Driver);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Driver>> GetAvailableDriversAsync()
        {
            return await _context.Drivers.Where(d => d.IsAvailable).ToListAsync();
        }

        public async Task UpdateAsync(Driver Driver)
        {
            _context.Drivers.Update(Driver);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Driver Driver)
        {

            _context.Drivers.Remove(Driver);
            await _context.SaveChangesAsync();

        }

    }
}
