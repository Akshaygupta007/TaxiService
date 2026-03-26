using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxiService.DataDb;
using TaxiService.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaxiService.DTOs.Requests;

namespace TaxiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DriversController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDriver([FromBody] CreateDriverRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingLiceseNumber = _context.Drivers
            .FirstOrDefault(d => d.LicenseNumber == request.LicenseNumber);

            if (existingLiceseNumber != null)
                return Conflict("License Number already exists");

            var phoneExists = await _context.Drivers
                .AnyAsync(d => d.PhoneNumber == request.PhoneNumber);
            if (phoneExists)
                return Conflict("A driver with this phone number already exists.");

            var driver = new Driver
            {
                Name = request.Name,
                LicenseNumber = request.LicenseNumber.Trim(),
                PhoneNumber = request.PhoneNumber.Trim(),
                CreatedAt = DateTime.UtcNow,
                IsAvailable = true
            };
            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateDriver), new { id = driver.DriverID }, new
            {
                message = "Driver created successfully.",
                driverId = driver.DriverID
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _context.Drivers.ToListAsync();
            return Ok(drivers);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriverById(int id)
        {
            var driver = await _context.Drivers
                         .Include(d => d.Vehicles)
                         .ThenInclude(dv => dv.Vehicle)
                         .FirstOrDefaultAsync(d => d.DriverID == id);

            if (driver == null)
                return NotFound($"Driver with ID {id} not found.");
            return Ok(driver);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdate(int id, [FromBody] UpdateDriverRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.DriverID == id);
            if (driver == null)
                return NotFound($"Driver with ID {id} not found.");
            // if more fields are added in future, we can use AutoMapper to map non-null fields from request to entity
            if (!string.IsNullOrEmpty(request.Name))
                driver.Name = request.Name.Trim();
            if (!string.IsNullOrEmpty(request.PhoneNumber))
                driver.PhoneNumber = request.PhoneNumber.Trim();
            if (!string.IsNullOrEmpty(request.LicenseNumber))
                driver.LicenseNumber = request.LicenseNumber.Trim();
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Driver updated successfully.",
                driverId = driver.DriverID
            });

        }


        [HttpGet("available-drivers")]
        public async Task<IActionResult> GetAvailableDrivers()
        {
            var availableDrivers = await _context.Drivers
                .Where(d => d.IsAvailable)
                .ToListAsync();
            return Ok(availableDrivers);
        }

        [HttpPost("toggle-availability/{id}")]
        public async Task<IActionResult> ToggleDriverAvailability(int id)
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.DriverID == id);
            if (driver == null)
                return NotFound($"Driver with ID {id} not found.");

            driver.IsAvailable = !driver.IsAvailable;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Driver availability toggled to {(driver.IsAvailable ? "available" : "unavailable")}.",
                driverId = driver.DriverID,
                isAvailable = driver.IsAvailable
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.DriverID == id);
            if (driver == null)
                return NotFound($"Driver with ID {id} not found.");

            // Check if driver has active vehicle assignments
            var hasVehicles = await _context.DriverVehicles
                .AnyAsync(dv => dv.DriverID == id);
            if (hasVehicles)
                return Conflict("Cannot delete driver — they are still assigned to vehicles.");

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Driver deleted successfully.",
                driverId = driver.DriverID
            });
        }
    }
}
