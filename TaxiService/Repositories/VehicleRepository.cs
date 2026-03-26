using Microsoft.EntityFrameworkCore;
using System.Numerics;
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

        public async Task<IList<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle?> GetByVehicleNumberAsync(string vehicleNumber)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleNumber == vehicleNumber);
        }

        public async Task<IList<Vehicle>> GetAvailableVehiclesAsync()
        {
            return await _context.Vehicles.Where(d => d.IsAvailable).ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            return vehicle;
        }

        public async Task AssignDriver(DriverVehicle driverVehicle)
        {
            await _context.DriverVehicles.AddAsync(driverVehicle);
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Vehicle vehicle)
        {

            _context.Vehicles.Remove(vehicle);
            await Task.CompletedTask;

        }
    }
}
