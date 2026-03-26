using Microsoft.EntityFrameworkCore;
using TaxiService.DataDb;
using TaxiService.Entities;
using TaxiService.Repositories.Interfaces;

namespace TaxiService.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task AssignDriver(DriverVehicle driverVehicle)
        {

            await _context.DriverVehicles.AddAsync(driverVehicle);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Vehicle vehicle)
        {

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

        }
    }
}
