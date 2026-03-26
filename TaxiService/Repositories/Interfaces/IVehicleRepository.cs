using TaxiService.Entities;

namespace TaxiService.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(int id);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task AddAsync(Vehicle vehicle);
        Task AssignDriver(DriverVehicle driverVehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Vehicle vehicle);
    }
}
