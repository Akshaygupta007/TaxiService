using TaxiService.Entities;

namespace TaxiService.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(int id);
        
        Task<IList<Vehicle>> GetAllAsync();
        
        Task<Vehicle> AddAsync(Vehicle vehicle);
        
        Task AssignDriver(DriverVehicle driverVehicle);

        Task<Vehicle?> GetByVehicleNumberAsync(string vehicleNumber);

        Task<IList<Vehicle>> GetAvailableVehiclesAsync();

        Task SaveChangesAsync();

        Task UpdateAsync(Vehicle vehicle);
        
        Task DeleteAsync(Vehicle vehicle);
    }
}
