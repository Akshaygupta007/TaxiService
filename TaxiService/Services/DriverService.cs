using TaxiService.Services.Interfaces;
using TaxiService.Repositories.Interfaces;
using TaxiService.DTOs.Responses;
using TaxiService.DTOs.Requests;
using AutoMapper;
using TaxiService.Entities;
using System.Runtime.CompilerServices;

namespace TaxiService.Services
{
    public class DriverService: IDriverService
    {
        private readonly ILogger<DriverService> _logger;
        private readonly IMapper _mapper;
        private readonly IDriverRepository _driverRepository;

        public DriverService(IDriverRepository driverRepository, ILogger<DriverService> logger, IMapper mapper)
        {
            _driverRepository = driverRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<DriverResponse> CreateDriverAsync(CreateDriverRequest request)
        {
            _logger.LogInformation("Creating new driver");
            if (request == null)
            {
                _logger.LogWarning("Create driver attempt with null request");
                throw new ArgumentNullException(nameof(request), "Create driver request cannot be null");
            }
            ValidateCreateDriverRequest(request);
            _logger.LogInformation("Check Phone Number or license number already exists");
            var existingDriverWithLicenseNumber = await _driverRepository.GetByLicenseAsync(request.LicenseNumber);
            var existingDriverWithPhoneNumber = await _driverRepository.GetByPhoneNumberAsync( request.PhoneNumber);
            if (existingDriverWithLicenseNumber != null)
            {
                _logger.LogWarning($"Driver with license number '{request.LicenseNumber}' already exists");
                throw new InvalidOperationException($"Driver with license number '{request.LicenseNumber}'' already exists");
            }
            if (existingDriverWithPhoneNumber != null)
            {
                _logger.LogWarning($"Driver with phone number '{request.PhoneNumber}' already exists");
                throw new InvalidOperationException($"Driver with phone number '{request.PhoneNumber}' already exists");
            }
            var driver = _mapper.Map<Driver>(request);
            driver.Rating = 0; // New drivers start with a rating of 0
            driver.TotalRides = 0; // New drivers start with 0 rides
            driver.IsAvailable = true; // New drivers are available by default

            var createdDriver = await _driverRepository.AddAsync(driver);
            await _driverRepository.SaveChangesAsync();

            _logger.LogInformation($"Driver created successfully: {createdDriver.DriverID}");

            return _mapper.Map<DriverResponse>(createdDriver);

        }

        public async Task<List<DriverResponse>> GetAvailableDriversAsync()
        {
            _logger.LogInformation("Fetching available drivers");
            var drivers = await _driverRepository.GetAvailableDriversAsync();
            if (drivers == null || drivers.Count == 0)
            {
                _logger.LogInformation("No available drivers found in system");
                return new List<DriverResponse>();
            }
            _logger.LogInformation($"Found {drivers.Count} available drivers in system");
            return _mapper.Map<List<DriverResponse>>(drivers);
        }

        public async Task<DriverResponse> UpdateDriverAsync(int driverId, UpdateDriverRequest request)
        {
            _logger.LogInformation($"Updating driver with ID: {driverId}");
            if (driverId <= 0)
            {
                _logger.LogWarning($"Update driver attempt with invalid ID: {driverId}");
                throw new ArgumentException("Driver ID must be a positive integer", nameof(driverId));
            }
            if (request == null)
            {
                _logger.LogWarning("Update driver attempt with null request");
                throw new ArgumentNullException(nameof(request), "Update driver request cannot be null");
            }
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                _logger.LogWarning($"Driver not found with ID: {driverId}");
                throw new KeyNotFoundException($"Driver with ID {driverId} not found");
            }
            if(!string.IsNullOrEmpty(request.PhoneNumber) && request.PhoneNumber != driver.PhoneNumber)
            {
                var existingDriverWithPhoneNumber = await _driverRepository.GetByPhoneNumberAsync(request.PhoneNumber);
                if (existingDriverWithPhoneNumber != null)
                {
                    _logger.LogWarning($"Driver with phone number '{request.PhoneNumber}' already exists");
                    throw new InvalidOperationException($"Driver with phone number '{request.PhoneNumber}' already exists");
                }
            }
            if (!string.IsNullOrEmpty(request.LicenseNumber) && request.LicenseNumber != driver.LicenseNumber)
            {
                var existingDriverWithLicense = await _driverRepository.GetByLicenseAsync(request.LicenseNumber);
                if (existingDriverWithLicense != null)
                {
                    _logger.LogWarning($"Driver with License number '{request.LicenseNumber}' already exists");
                    throw new InvalidOperationException($"Driver with License number '{request.LicenseNumber}' already exists");
                }
            }
            // Update driver properties
            driver.Name = request.Name ?? driver.Name;
            driver.PhoneNumber = request.PhoneNumber?.Trim() ?? driver.PhoneNumber;
            driver.LicenseNumber = request.LicenseNumber?.Trim() ?? driver.LicenseNumber;
            if (request.IsAvailable.HasValue)
                driver.IsAvailable = request.IsAvailable.Value;
            if (request.Rating.HasValue && request.Rating.Value>=0)
                driver.Rating = request.Rating.Value;
            if (request.TotalRides.HasValue && request.TotalRides.Value>=0)
                driver.TotalRides = request.TotalRides.Value;
            await _driverRepository.UpdateAsync(driver);
            await _driverRepository.SaveChangesAsync();
            
            _logger.LogInformation($"Driver updated successfully: {driverId}");
            return _mapper.Map<DriverResponse>(driver);
        }

        public async Task<DriverResponse> ToggleAvailabilityAsync(int driverId)
        {
            _logger.LogInformation($"Toggling availability for driver with ID: {driverId}");
            if (driverId <= 0)
            {
                _logger.LogWarning($"Toggle availability attempt with invalid ID: {driverId}");
                throw new ArgumentException("Driver ID must be a positive integer", nameof(driverId));
            }
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                _logger.LogWarning($"Driver not found with ID: {driverId}");
                throw new KeyNotFoundException($"Driver with ID {driverId} not found");
            }
            driver.IsAvailable = !driver.IsAvailable;
            await _driverRepository.UpdateAsync(driver);
            await _driverRepository.SaveChangesAsync();
            _logger.LogInformation($"Driver availability toggled successfully: {driverId} is now {(driver.IsAvailable ? "available" : "unavailable")}");
            return _mapper.Map<DriverResponse>(driver);
        }

        public async Task<List<DriverResponse>> GetAllDriversAsync()
        {
            _logger.LogInformation("Fetching all cab types");
            var drivers = await _driverRepository.GetAllAsync();
            if (drivers == null || drivers.Count == 0)
            {
                _logger.LogInformation("No drivers found in system");
                return new List<DriverResponse>();
            }
            _logger.LogInformation($"Found {drivers.Count} drivers in system");
            return _mapper.Map<List<DriverResponse>>(drivers);
        }

        public async Task<DriverResponse> GetDriverByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Get driver by ID attempt with invalid ID: {id}");
                throw new ArgumentException("Driver ID must be a positive integer", nameof(id));
            }
            var driver = await _driverRepository.GetByIdAsync(id);
            if (driver == null)
            {
                _logger.LogWarning($"Driver not found with ID: {id}");
                throw new KeyNotFoundException($"Driver with ID {id} not found");
            }
            _logger.LogInformation($"Cab type found with ID: {id}");
            return _mapper.Map<DriverResponse>(driver);
        }

        public async  Task DeleteDriverAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Delete driver attempt with invalid ID: {id}");
                throw new ArgumentException("Driver ID must be a positive integer", nameof(id));
            }
            var driver = await _driverRepository.GetByIdAsync(id);
            if (driver == null)
            {
                _logger.LogWarning($"Delete attempt for non-existent driver: {id}");
                throw new KeyNotFoundException($"Driver with ID {id} not found");
            }
            await _driverRepository.DeleteAsync(driver);
            await _driverRepository.SaveChangesAsync();
            _logger.LogInformation($"Driver deleted successfully: {id}");
        }

        private void ValidateCreateDriverRequest(CreateDriverRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name is required", nameof(request.Name));

            if (string.IsNullOrWhiteSpace(request.LicenseNumber))
                throw new ArgumentException("LicenseNumber is required", nameof(request.LicenseNumber));

            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                throw new ArgumentException("PhoneNumber is required", nameof(request.PhoneNumber));

        }
    }
}
