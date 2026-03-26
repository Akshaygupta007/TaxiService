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
    public class CabTypeController : ControllerBase
    {
        private readonly ICabTypeService _cabTypeService;
        private readonly ILogger<CabTypeController> _logger;

        public CabTypeController(ICabTypeService cabTypeService, ILogger<CabTypeController> logger)
        {
            _cabTypeService = cabTypeService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCabType([FromBody] CreateCabTypeRequest request)
        {

            _logger.LogInformation("CreateCabType endpoint called");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _cabTypeService.CreateCabTypeAsync(request);
            _logger.LogInformation($"Cab type created successfully with ID: {result.CabTypeID}");


            return CreatedAtAction(nameof(CreateCabType), new { id = result.CabTypeID }, new
            {
                message = "Cab type created successfully.",
                cabTypeId = result.CabTypeID
            });

        }
        [HttpGet]
        public async Task<IActionResult> GetAllCabTypes()
        {
            _logger.LogInformation("GetAllCabTypes endpoint called");
            var cabTypes = await _cabTypeService.GetAllCabTypesAsync();
            _logger.LogInformation($"Retrieved {cabTypes.Count} cab types.");
            return Ok(cabTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCabTypeById(int id)
        {
            _logger.LogInformation($"GetCabTypeById endpoint called with ID: {id}");
            var cabType = await _cabTypeService.GetCabTypeIdAsync(id);
            _logger.LogInformation($"Retrieved cab type with ID: {id}");
            return Ok(cabType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCabTypeById(int id, [FromBody] UpdateCabTypeRequest request)
        {
            _logger.LogInformation($"UpdateCabTypeById endpoint called with ID: {id}");
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var cabType = await _cabTypeService.UpdateCabType(id, request);
            _logger.LogInformation($"CabType with ID: {id} updated successfully.");

            return Ok(new
            {
                message = "CabType updated successfully.",
                cabType
            });

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            _logger.LogInformation($"DeleteCabType endpoint called with ID: {id}");
            await _cabTypeService.DeleteUserAsync(id);
            _logger.LogInformation($"CabType with ID: {id} deleted successfully.");
            return Ok(new
            {
                message = "CabType deleted successfully.",
                CabTypeId = id
            });
        }
    }

}
