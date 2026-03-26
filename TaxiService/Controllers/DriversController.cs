using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxiService.DataDb;
using TaxiService.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaxiService.DTOs.Requests;
using TaxiService.Services.Interfaces;

namespace TaxiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly ILogger<DriversController> _logger;

        public DriversController(IDriverService driverService, ILogger<DriversController> logger)
        {
            _driverService = driverService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDriver([FromBody] CreateDriverRequest request)
        {
            _logger.LogInformation("CreateDriver endpoint called");
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _driverService.CreateDriverAsync(request);

            _logger.LogInformation($"Driver created successfully with ID: {result.DriverID}");


            return CreatedAtAction(nameof(CreateDriver), new { id = result.DriverID }, new
            {
                message = "Driver created successfully.",
                driverId = result.DriverID
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            _logger.LogInformation("GetAllDrivers endpoint called");
            var drivers = await _driverService.GetAllDriversAsync();
            _logger.LogInformation($"Retrieved {drivers.Count} drivers.");
            return Ok(drivers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverById(int id)
        {
            _logger.LogInformation($"GetDriverById endpoint called with ID: {id}");
            var driver = await _driverService.GetDriverByIdAsync(id);
            _logger.LogInformation($"Retrieved driver with ID: {id}");
            return Ok(driver);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDriverRequest request)
        {
            _logger.LogInformation($"UpdateDriver endpoint called with ID: {id}");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var driver = await _driverService.UpdateDriverAsync(id, request);
            _logger.LogInformation($"Driver updated successfully with ID: {id}");

            return Ok(new
            {
                message = "Driver updated successfully.",
                driverId = driver.DriverID
            });

        }

        [HttpGet("available-drivers")]
        public async Task<IActionResult> GetAvailableDrivers()
        {

            _logger.LogInformation("GetAvailableDrivers endpoint called");
            var drivers = await _driverService.GetAvailableDriversAsync();
            _logger.LogInformation($"Retrieved {drivers.Count} available drivers.");
            return Ok(drivers);
        }

        [HttpPost("toggle-availability/{id}")]
        public async Task<IActionResult> ToggleDriverAvailability(int id)
        {
            _logger.LogInformation($"ToggleDriverAvailability endpoint called with ID: {id}");
            var driver = await _driverService.ToggleAvailabilityAsync(id);
            _logger.LogInformation($"Driver availability toggled successfully for ID: {id} to {(driver.IsAvailable ? "available" : "unavailable")}");

            return Ok(driver);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {

            _logger.LogInformation($"DeleteDriver endpoint called with ID: {id}");
            await _driverService.DeleteDriverAsync(id);
            _logger.LogInformation($"Driver deleted successfully with ID: {id}");
            return Ok(new
            {
                message = "Driver deleted successfully.",
                driverId = id
            });
        }
    }
}
