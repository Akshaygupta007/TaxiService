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
    public class VehiclesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check cab type exists
            var cabType = _context.CabTypes.FirstOrDefault(c => c.CabTypeID == request.CabTypeID);
            if (cabType == null)
                return NotFound("Cab type not found");

            //Prevent duplicate vehicle number
            var existingVehicle = _context.Vehicles
                .FirstOrDefault(v => v.VehicleNumber == request.VehicleNumber);
            if (existingVehicle != null)
                return Conflict("Vehicle already exists");

            var vehicle = new Vehicle
            {
                CabTypeID = request.CabTypeID,
                VehicleNumber = request.VehicleNumber.Trim(),
                VehicleModel = request.VehicleModel.Trim(),
                ManufactureYear = request.ManufactureYear,
                Color = request.Color.Trim()

            };
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(CreateVehicle), new { id = vehicle.VehicleID }, new
            {
                message = "Vehicle created successfully.",
                vehicleId = vehicle.VehicleID
            });
        }

        [HttpPost("assign-driver")]
        public async Task<IActionResult> AssignDriver([FromBody] AssignDriverRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //check if driver exists
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.DriverID == request.DriverId);
            if (driver == null)
                return NotFound($"Driver with ID {request.DriverId} not found.");

            //check if vehicle exists
            var vehicleExists = await _context.Vehicles.AnyAsync(v => v.VehicleID == request.VehicleId);
            if (!vehicleExists)
                return NotFound($"Vehicle with ID {request.VehicleId} not found.");

            //check if assignment already exists
            var alreadyAssigned = await _context.DriverVehicles
                .AnyAsync(dv => dv.DriverID == request.DriverId && dv.VehicleID == request.VehicleId);
            if (alreadyAssigned)
                return Conflict("Driver is already assigned to this vehicle.");

            // Check if there's already a primary driver for this vehicle
            var primaryDriver = await _context.DriverVehicles
                .FirstOrDefaultAsync(dv => dv.VehicleID == request.VehicleId && dv.IsPrimaryDriver);

            bool isPrimary = false;
            if (primaryDriver == null)
                isPrimary = true; // If no primary driver, make this one primary by default
            else if (primaryDriver != null && driver.IsAvailable)
            {
                return Conflict(new
                {
                    message = "Cannot assign a new driver, Primary driver is still available.",
                    primaryDriverId = primaryDriver.DriverID
                });
            }
            else if (primaryDriver != null && !driver.IsAvailable)
            {
                isPrimary = true; // If existing primary driver is not available, promote new driver as backup
            }
            var driverVehicle = new DriverVehicle
            {
                DriverID = request.DriverId,
                VehicleID = request.VehicleId,
                IsPrimaryDriver = isPrimary,
                AssignedAt = DateTime.UtcNow
            };
            await _context.DriverVehicles.AddAsync(driverVehicle);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Driver assigned to vehicle successfully."
            });

        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.CabType) // Include CabType details
                .ToListAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehcicleById(int id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.CabType) // Include CabType details
                .FirstOrDefaultAsync(v => v.VehicleID == id);
            if (vehicle == null)
                return NotFound($"Vehicle with ID {id} not found.");
            return Ok(vehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] UpdateVehicleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleID == id);
            if (vehicle == null)
                return NotFound($"Vehicle with ID {id} not found.");
            // if more fields are added in future, we can use AutoMapper to map non-null fields from request to entity
            vehicle.ManufactureYear = (int)request.ManufactureYear;
            vehicle.Color = request.Color.Trim();
            vehicle.VehicleModel = request.VehicleModel.Trim();
            vehicle.VehicleNumber = request.VehicleNumber.Trim();
            if (request.CabTypeID.HasValue)
            {
                // Check if the new cab type exists
                var cabType = await _context.CabTypes.FirstOrDefaultAsync(c => c.CabTypeID == request.CabTypeID.Value);
                if (cabType == null)
                    return NotFound($"Cab type with ID {request.CabTypeID.Value} not found.");
                vehicle.CabTypeID = request.CabTypeID.Value;
            }
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Vehicle updated successfully.",
                vehicle = new
                {
                    vehicle.VehicleID,
                    vehicle.CabTypeID,
                    vehicle.VehicleNumber,
                    vehicle.VehicleModel,
                    vehicle.ManufactureYear,
                    vehicle.Color
                }
            });


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleByID(int id)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleID == id);
            if (vehicle == null)
                return NotFound($"Vehicle with ID {id} not found.");
            //check if vehicle is assigned to any driver
            var isAssigned = await _context.DriverVehicles.AnyAsync(dv => dv.VehicleID == id);
            if (isAssigned)
                return Conflict("Cannot delete vehicle because it is assigned to one or more drivers.");

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Vehicle deleted successfully.",
                vehicleID = vehicle.VehicleID
            });

        }
    }
}