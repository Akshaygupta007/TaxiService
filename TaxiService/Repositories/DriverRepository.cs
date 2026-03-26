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
        public async Task<Driver?> GetByLicenseAsync(string licenseNumber)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.LicenseNumber == licenseNumber);
        }

        public async Task<Driver?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.PhoneNumber == phoneNumber);
        }
        public async Task<IList<Driver>> GetAllAsync()
        {
            return await _context.Drivers
                    .Include(d => d.Vehicle)
                    .ThenInclude(v => v.CabType)
                    .ToListAsync();
        }

        public async Task<Driver> AddAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver);
            return driver;
        }

        public async Task<IList<Driver>> GetAvailableDriversAsync()
        {
            return await _context.Drivers.Where(d => d.IsAvailable).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Driver driver)
        {
            _context.Drivers.Remove(driver);
            await Task.CompletedTask;
        }
    }
}
