using Microsoft.AspNetCore.Mvc;
using TaxiService.DataDb;
using TaxiService.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaxiService.DTOs.Requests;
using TaxiService.Services.Interfaces;
using TaxiService.Services;
namespace TaxiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehicleService vehicleService, ILogger<VehiclesController> logger)
        {
            _vehicleService = vehicleService;
            _logger = logger;
        }

            [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("CreateVehicle endpoint called");
            var result = await _vehicleService.CreateVehicleAsync(request);

            _logger.LogInformation($"Vehicle created successfully with ID: {result.VehicleID}");

            return CreatedAtAction(nameof(CreateVehicle), new { id = result.VehicleID }, new
            {
                message = "Vehicle created successfully.",
                vehicleId = result.VehicleID
            });
        }

        [HttpPost("assign-driver")]
        public async Task<IActionResult> AssignDriver([FromBody] AssignDriverRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _logger.LogInformation("AssignDriver endpoint called");

            var result = await _vehicleService.AssignDriverToVehicleAsync(request);
            _logger.LogInformation($"Driver with ID: {request.DriverID} assigned to vehicle with ID: {request.VehicleID} successfully");

            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            _logger.LogInformation("GetAllVehicles endpoint called");
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            _logger.LogInformation($"Retrieved {vehicles.Count} vehicles.");
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehcicleById(int id)
        {
            _logger.LogInformation($"GetVehicleById endpoint called with ID: {id}");
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            _logger.LogInformation($"Retrieved vehicle with ID: {id}");

            return Ok(vehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] UpdateVehicleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation($"UpdateVehicle endpoint called with ID: {id}");
            var result = await _vehicleService.UpdateVehicleAsync(id, request);
            _logger.LogInformation($"Vehicle with ID: {id} updated successfully");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleByID(int id)
        {
            _logger.LogInformation($"DeleteVehicleByID endpoint called with ID: {id}");
            await _vehicleService.DeleteVehicleAsync(id);
            _logger.LogInformation($"Vehicle with ID: {id} deleted successfully");

            return Ok(new
            {
                message = "Vehicle deleted successfully.",
                vehicleId = id
            });

        }
    }
}