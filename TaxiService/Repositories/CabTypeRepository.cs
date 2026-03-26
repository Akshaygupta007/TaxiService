using Microsoft.EntityFrameworkCore;
using TaxiService.DataDb;
using TaxiService.Entities;
using TaxiService.Repositories.Interfaces;

namespace TaxiService.Repositories
{
    public class CabTypeRepository : ICabTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public CabTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CabType?> GetByIdAsync(int id)
        {
            return await _context.CabTypes.FindAsync(id);
        }
        public async Task<IEnumerable<CabType>> GetAllAsync()
        {
            return await _context.CabTypes.ToListAsync();
        }
        public async Task AddAsync(CabType cabType)
        {
            await _context.CabTypes.AddAsync(cabType);
            await _context.SaveChangesAsync();

        }
        public async Task UpdateAsync(CabType cabType)
        {
            _context.CabTypes.Update(cabType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CabType cabType)
        {
            _context.CabTypes.Remove(cabType);
            await _context.SaveChangesAsync();
        }
    }
}
