using TaxiService.Services.Interfaces;
using TaxiService.Repositories.Interfaces;
using TaxiService.DTOs.Responses;
using TaxiService.DTOs.Requests;
using AutoMapper;
using TaxiService.Entities;


namespace TaxiService.Services
{
    public class CabTypeService: ICabTypeService
    {
        private readonly ILogger<CabTypeService> _logger;
        private readonly IMapper _mapper;
        private readonly ICabTypeRepository _cabTypeRepository;

        public CabTypeService(ILogger<CabTypeService> logger, IMapper mapper, ICabTypeRepository cabTypeRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _cabTypeRepository = cabTypeRepository;
        }

        public async Task<CabTypeResponse> CreateCabTypeAsync(CreateCabTypeRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("Create cab type attempt with null request");
                throw new ArgumentNullException(nameof(request), "Create cab type request cannot be null");
            }
            ValidateCreateCabTypeRequest(request);

            var existingCabType = await _cabTypeRepository.CabTypeNameExistsAsync(request.CabTypeName);
            if (existingCabType!=null)
            {
                _logger.LogWarning($"Cab type name '{request.CabTypeName}' is already in use");
                throw new InvalidOperationException($"Cab type name '{request.CabTypeName}' is already in use");
            }
            var cabType = _mapper.Map<CabType>(request);

            var createdCabType = await _cabTypeRepository.AddAsync(cabType);
            await _cabTypeRepository.SaveChangesAsync();

            _logger.LogInformation($"CabType created successfully with ID: {createdCabType.CabTypeID}");
            return _mapper.Map<CabTypeResponse>(createdCabType);
        }

        public async Task<List<CabTypeResponse>> GetAllCabTypesAsync()
        {
            _logger.LogInformation("Fetching all cab types");
            var cabTypes = await _cabTypeRepository.GetAllAsync();
            if(cabTypes == null || cabTypes.Count == 0)
            {
                _logger.LogInformation("No users found in system");
                return new List<CabTypeResponse>();
            }
            _logger.LogInformation($"Found {cabTypes.Count} cab types in system");
            return _mapper.Map<List<CabTypeResponse>>(cabTypes);

        }

        public async Task<CabTypeResponse> GetCabTypeIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Invalid cab type ID: {id}");
                throw new ArgumentException("Cab type ID must be greater than zero", nameof(id));
            }
            var cabType = await _cabTypeRepository.GetByIdAsync(id);
            if (cabType == null)
            {
                _logger.LogWarning($"Cab type not found with ID: {id}");
                throw new KeyNotFoundException($"Cab type with ID {id} not found");
            }
            _logger.LogInformation($"Cab type found with ID: {id}");
            return _mapper.Map<CabTypeResponse>(cabType);
        }

        public async Task<CabTypeResponse> UpdateCabType(int cabTypeId, UpdateCabTypeRequest cabTypeRequest)
        {
            if (cabTypeId <= 0)
            {
                _logger.LogWarning($"Invalid cab type ID: {cabTypeId}");
                throw new ArgumentException("Cab type ID must be greater than zero", nameof(cabTypeId));
            }
            if (cabTypeRequest == null)
            {
                _logger.LogWarning("Update cab type attempt with null request");
                throw new ArgumentNullException(nameof(cabTypeRequest), "Update cab type request cannot be null");
            }

            var cabType = await _cabTypeRepository.GetByIdAsync(cabTypeId);
            if (cabType == null)
            {
                _logger.LogWarning($"Cab type not found with ID: {cabTypeId}");
                throw new KeyNotFoundException($"Cab type with ID {cabTypeId} not found");
            }

            if (cabTypeRequest.BaseFare.HasValue && cabTypeRequest.BaseFare.Value > 0)
                cabType.BaseFare = cabTypeRequest.BaseFare.Value;

            if (cabTypeRequest.FarePerKm.HasValue && cabTypeRequest.FarePerKm.Value > 0)
                cabType.FarePerKm = cabTypeRequest.FarePerKm.Value;
            if ( !string.IsNullOrEmpty(cabTypeRequest.CabTypeName) && cabTypeRequest.CabTypeName != cabType.CabTypeName)
            {
                // Check if another cab type with the same name already exists
                var existingCabType = await _cabTypeRepository.CabTypeNameExistsAsync(cabTypeRequest.CabTypeName);
                if (existingCabType != null)
                {
                    _logger.LogWarning($"Cab type name '{cabTypeRequest.CabTypeName}' is already in use");
                    throw new InvalidOperationException($"Cab type name '{cabTypeRequest.CabTypeName}' is already in use");
                }
                cabType.CabTypeName = cabTypeRequest.CabTypeName.Trim();
            }

            await _cabTypeRepository.UpdateAsync(cabType);
            await _cabTypeRepository.SaveChangesAsync();

            _logger.LogInformation($"CabType {cabTypeId} updated successfully");

            return _mapper.Map<CabTypeResponse>(cabType);

        }

        public async Task DeleteUserAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Invalid cab type ID: {id}");
                throw new ArgumentException("Cab type ID must be greater than zero", nameof(id));
            }
            var cabType = await _cabTypeRepository.GetByIdAsync(id);
            if (cabType == null)
            {
                _logger.LogWarning($"Cab type not found with ID: {id}");
                throw new KeyNotFoundException($"Cab type with ID {id} not found");
            }
            await _cabTypeRepository.DeleteAsync(cabType);
            await _cabTypeRepository.SaveChangesAsync();
            _logger.LogInformation($"Cab type with ID {id} deleted successfully");
        }

        private void ValidateCreateCabTypeRequest(CreateCabTypeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CabTypeName))
                throw new ArgumentException("CabTypeName is required", nameof(request.CabTypeName));

            if (request.BaseFare <= 0)
                throw new ArgumentException("BaseFare must be greater than 0", nameof(request.BaseFare));

            if (request.FarePerKm <= 0)
                throw new ArgumentException("FarePerKm must be greater than 0", nameof(request.FarePerKm));
        }
    }
}
