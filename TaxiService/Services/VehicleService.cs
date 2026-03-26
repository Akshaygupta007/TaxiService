using TaxiService.Services.Interfaces;
using TaxiService.Repositories.Interfaces;
using TaxiService.DTOs.Responses;
using TaxiService.DTOs.Requests;
using AutoMapper;
using TaxiService.Entities;

namespace TaxiService.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ICabTypeRepository _cabTypeRespository;
        private readonly IDriverRepository _driverRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public VehicleService(IVehicleRepository vehicleRepository, IDriverRepository driverRepository, ICabTypeRepository cabTypeRespository, IMapper mapper, ILogger<VehicleService> logger)
        {
            _vehicleRepository = vehicleRepository;
            _cabTypeRespository = cabTypeRespository;
            _driverRepository = driverRepository;
            _mapper = mapper;
           _logger = logger;
        }

        public async Task<VehicleResponse> CreateVehicleAsync(CreateVehicleRequest request)
        {
            _logger.LogInformation("Creating a new vehicle with Vehicle Number {VehicleNumber}", request.VehicleNumber);
            if (request == null)
            {
                _logger.LogWarning("CreateVehicleAsync called with null request");
                throw new ArgumentNullException(nameof(request), "CreateVehicleRequest cannot be null");
            }
            ValidateCreateVehicleRequest(request);
            _logger.LogInformation("Check Vehicle Number already exists");
            var existingVehicle = await _vehicleRepository.GetByVehicleNumberAsync(request.VehicleNumber);
            if (existingVehicle!= null)
            {
                _logger.LogWarning("Vehicle with Vehicle Number {VehicleNumber} already exists", request.VehicleNumber);
                throw new InvalidOperationException($"Vehicle with Vehicle Number {request.VehicleNumber} already exists");
            }

            _logger.LogInformation("Check Cab Type exists for Cab Type ID {CabTypeID}", request.CabTypeID);
            var cabType = await _cabTypeRespository.GetByIdAsync(request.CabTypeID);
            if (cabType!= null)
            {
                _logger.LogWarning("Cab Type with ID {CabTypeID} not found", request.CabTypeID);
                throw new InvalidOperationException($"Cab Type with ID '{request.CabTypeID}'' not found ");
            }
            var vehicleEntity = _mapper.Map<Vehicle>(request);
            var createdVehicle = await _vehicleRepository.AddAsync(vehicleEntity);
            
            await _vehicleRepository.SaveChangesAsync();
            _logger.LogInformation("Vehicle with Vehicle Number {VehicleNumber} created successfully with ID {VehicleID}", createdVehicle.VehicleNumber, createdVehicle.VehicleID);

            return _mapper.Map<VehicleResponse>(createdVehicle);
        }


        public async Task<VehicleResponse> AssignDriverToVehicleAsync(AssignDriverRequest request)
        {
            _logger.LogInformation("Assigning driver with ID {DriverID} to vehicle with ID {VehicleID}", request.DriverID, request.VehicleID);
            if (request == null)
            {
                _logger.LogWarning("AssignDriverToVehicleAsync called with null request");
                throw new ArgumentNullException(nameof(request), "AssignDriverRequest cannot be null");
            }
            if (request.VehicleID <= 0)
            {
                _logger.LogWarning("Invalid vehicle ID for driver assignment: {VehicleID}", request.VehicleID);
                throw new ArgumentException("Invalid vehicle ID for driver assignment", nameof(request.VehicleID));
            }
            if (request.DriverID <= 0)
            {
                _logger.LogWarning("Invalid driver ID for driver assignment: {DriverID}", request.DriverID);
                throw new ArgumentException("Invalid driver ID for driver assignment", nameof(request.DriverID));
            }
            //need to check if driver and vehicle exists and then assign driver to vehicle
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleID);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with ID {VehicleID} not found for driver assignment", request.VehicleID);
                throw new InvalidOperationException($"Vehicle with ID {request.VehicleID} not found for driver assignment");
            }
            var driver = await _driverRepository.GetByIdAsync(request.DriverID);
            if (driver == null)
            {
                _logger.LogWarning("Driver with ID {DriverID} not found for driver assignment", request.DriverID);
                throw new InvalidOperationException($"Driver with ID {request.DriverID} not found for driver assignment");
            }
            // Check if driver is already assigned to a vehicle
            if (driver.VehicleId.HasValue)
            {
                _logger.LogWarning("Driver with ID {DriverID} is already assigned to vehicle with ID {VehicleID}", request.DriverID, driver.VehicleId.Value);
                throw new InvalidOperationException($"Driver with ID {request.DriverID} is already assigned to a vehicle");
            }
            // Check if vehicle already has a driver assigned
            if (vehicle.DriverId.HasValue)
            {
                _logger.LogWarning("Vehicle with ID {VehicleID} already has a driver assigned with ID {DriverID}", request.VehicleID, vehicle.DriverId.Value);
                throw new InvalidOperationException($"Vehicle with ID {request.VehicleID} already has a driver assigned");
            }
            // Assign driver to vehicle
            vehicle.DriverId = request.DriverID;
            driver.VehicleId = request.VehicleID;
            // Update driver and vehicle entities
            await _vehicleRepository.UpdateAsync(vehicle);
            await _driverRepository.UpdateAsync(driver);
            
            await _vehicleRepository.SaveChangesAsync();
            await _driverRepository.SaveChangesAsync();
            _logger.LogInformation("Driver with ID {DriverID} assigned to vehicle with ID {VehicleID} successfully", request.DriverID, request.VehicleID);
            return _mapper.Map<VehicleResponse>(vehicle);
        }


        public async Task<List<VehicleResponse>> GetAllVehiclesAsync()
        {
            _logger.LogInformation("Retrieving all vehicles");
            var vehicles = await _vehicleRepository.GetAllAsync();
            if (vehicles == null || vehicles.Count == 0)
            {
                _logger.LogInformation("No vehicles found");
                return new List<VehicleResponse>();
            }
            _logger.LogInformation("Retrieved {VehicleCount} vehicles", vehicles.Count);
            return _mapper.Map<List<VehicleResponse>>(vehicles);
        }

        public async Task<List<VehicleResponse>> GetAvailableVehiclesAsync()
        {
            _logger.LogInformation("Retrieving available vehicles");
            var vehicles = await _vehicleRepository.GetAvailableVehiclesAsync();
            if (vehicles == null || vehicles.Count == 0)
            {
                _logger.LogInformation("No available vehicles found");
                return new List<VehicleResponse>();
            }
            _logger.LogInformation("Retrieved {VehicleCount} available vehicles", vehicles.Count);
            return _mapper.Map<List<VehicleResponse>>(vehicles);
        }

        public async Task<VehicleResponse>  GetVehicleByIdAsync(int vehicleId)
        {
            _logger.LogInformation("Retrieving vehicle with ID {VehicleID}", vehicleId);
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with ID {VehicleID} not found", vehicleId);
                throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found");
            }
            _logger.LogInformation("Vehicle with ID {VehicleID} retrieved successfully", vehicleId);
            return _mapper.Map<VehicleResponse>(vehicle);
        }

        public async Task<VehicleResponse>  UpdateVehicleAsync(int vehicleId, UpdateVehicleRequest request)
        {
            _logger.LogInformation("Updating vehicle with ID {VehicleID}", vehicleId);
            if (vehicleId <= 0)
            {
                _logger.LogWarning("Invalid vehicle ID for update: {VehicleID}", vehicleId);
                throw new ArgumentException("Invalid vehicle ID for update", nameof(vehicleId));
            }
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with ID {VehicleID} not found for update", vehicleId);
                throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found for update");
            }
            if(!string.IsNullOrEmpty(vehicle.VehicleNumber) && vehicle.VehicleNumber != request.VehicleNumber) {
                
                _logger.LogInformation("Check Vehicle Number already exists for update");
                var existingVehicle = await _vehicleRepository.GetByVehicleNumberAsync(request.VehicleNumber);
                if (existingVehicle != null)
                {
                    _logger.LogWarning("Vehicle with Vehicle Number {VehicleNumber} already exists for update", request.VehicleNumber);
                    throw new InvalidOperationException($"Vehicle with Vehicle Number {request.VehicleNumber} already exists");
                }
            }
            vehicle.VehicleNumber = request.VehicleNumber ?? vehicle.VehicleNumber;
            vehicle.VehicleModel = request.VehicleModel ?? vehicle.VehicleModel;
            vehicle.ManufactureYear = request.ManufactureYear ?? vehicle.ManufactureYear;
            vehicle.Color = request.Color ?? vehicle.Color;
            vehicle.UpdatedAt = DateTime.UtcNow;
            if(request.IsAvailable.HasValue)
            {
                vehicle.IsAvailable = request.IsAvailable.Value;
            }
            if (request.Status.HasValue)
            {
                vehicle.Status = request.Status.Value;
            }

            await _vehicleRepository.UpdateAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();
            _logger.LogInformation("Vehicle with ID {VehicleID} updated successfully", vehicleId);
            return _mapper.Map<VehicleResponse>(vehicle);
        }

        public async Task DeleteVehicleAsync(int vehicleId)
        {
            if (vehicleId <= 0)
            {
                _logger.LogWarning("Invalid vehicle ID for deletion: {VehicleID}", vehicleId);
                throw new ArgumentException("Invalid vehicle ID for deletion", nameof(vehicleId));
            }
            _logger.LogInformation("Deleting vehicle with ID {VehicleID}", vehicleId);
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with ID {VehicleID} not found for deletion", vehicleId);
                throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found for deletion");
            }
            await _vehicleRepository.DeleteAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();
            _logger.LogInformation("Vehicle with ID {VehicleID} deleted successfully", vehicleId);
        }

        private void ValidateCreateVehicleRequest(CreateVehicleRequest request)
        {
            if(string.IsNullOrEmpty(request.VehicleNumber))
            {
                _logger.LogWarning("Vehicle number is required");
                throw new ArgumentException("Vehicle number is required", nameof(request.VehicleNumber));
            }
            if(string.IsNullOrEmpty(request.VehicleModel))
            {
                _logger.LogWarning("Vehicle model is required");
                throw new ArgumentException("Vehicle model is required", nameof(request.VehicleModel));
            }
            if (request.CabTypeID <= 0)
            {
                _logger.LogWarning("Invalid cab type ID for vehicle creation: {CabTypeID}", request.CabTypeID);
                throw new ArgumentException("Invalid cab type ID for vehicle creation", nameof(request.CabTypeID));
            }
            if(request.ManufactureYear<=1900 && request.ManufactureYear >= 2050)
            {
                _logger.LogWarning("Invalid manufacturing year for vehicle creation: {ManufactureYear}", request.ManufactureYear);
                throw new ArgumentException("Invalid manufacturing year for vehicle creation: {ManufactureYear}", nameof(request.ManufactureYear));
            }
        }
    }
}
