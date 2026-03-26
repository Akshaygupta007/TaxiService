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
    public class CabTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CabTypeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCabType([FromBody] CreateCabTypeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCabType = _context.CabTypes
                .FirstOrDefault(c => c.CabTypeName.ToLower() == request.CabTypeName.ToLower());
            if (existingCabType != null)
                return Conflict("Cab type already exists");

            var cabType = new CabType
            {
                CabTypeName = request.CabTypeName.Trim(),
                BaseFare = request.BaseFare,
                FarePerKm = request.FarePerKm
            };
            await _context.CabTypes.AddAsync(cabType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateCabType), new { id = cabType.CabTypeID }, new
            {
                message = "Cab type created successfully.",
                cabTypeId = cabType.CabTypeID
            });

        }
        [HttpGet]
        public async Task<IActionResult> GetAllCabTypes()
        {
            var cabTypes = await _context.CabTypes
                                .Select(c => new
                                {
                                    c.CabTypeID,
                                    c.CabTypeName,
                                    c.BaseFare,
                                    c.FarePerKm
                                })
                                .ToListAsync();
            return Ok(cabTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCabTypeById(int id)
        {
            var cabType = await _context.CabTypes
                                .Where(c => c.CabTypeID == id)
                                .Select(c => new
                                {
                                    c.CabTypeID,
                                    c.CabTypeName,
                                    c.BaseFare,
                                    c.FarePerKm
                                })
                                .FirstOrDefaultAsync();
            if (cabType == null)
                return NotFound($"CabType with ID {id} not found.");
            return Ok(cabType);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdate(int id, [FromBody] UpdateCabTypeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cabType = await _context.CabTypes.FirstOrDefaultAsync(c=>c.CabTypeID==id);
            if (cabType == null)
                return NotFound($"CabType with ID {id} not found.");
            // if more fields are added in future, we can use AutoMapper to map non-null fields from request to entity
            if (request.CabTypeName != null)
                cabType.CabTypeName = request.CabTypeName.Trim();
            if (request.BaseFare.HasValue)
                cabType.BaseFare = request.BaseFare.Value;
            if (request.FarePerKm.HasValue)
                cabType.FarePerKm = request.FarePerKm.Value;

            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "CabType updated successfully.",
                cabType
            });

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cabType = await _context.CabTypes.FirstOrDefaultAsync(c => c.CabTypeID == id);
            if (cabType == null)
                return NotFound($"CabType with ID {id} not found.");

            // Check if any vehicles are using this cab type
            bool isInUse = await _context.Vehicles.AnyAsync(v => v.CabTypeID == id);
            if (isInUse)
                return Conflict("Cannot delete cab type because it is assigned to one or more vehicles.");
           
            _context.CabTypes.Remove(cabType);
            await _context.SaveChangesAsync();
            
            return Ok(new
            {
                message = "CabType deleted successfully.",
                cabTypeID = cabType.CabTypeID
            });
        }
    }

}
