using Microsoft.AspNetCore.Mvc;
using TaxiService.DataDb;
using TaxiService.Entities;

using Microsoft.EntityFrameworkCore;
using TaxiService.Services.Interfaces;
using TaxiService.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;

namespace TaxiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController( IUserService userService, ILogger<UserController> logger)
        {

            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Register endpoint called");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for registration");
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterAsync(request);

            _logger.LogInformation($"User registered successfully with ID: {result.UserID}");

            return CreatedAtAction(nameof(Register), new { id = result.UserID }, new
            {
                message = "User registered successfully.",
                userId = result.UserID
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            _logger.LogInformation("Login endpoint called");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for login");
                return BadRequest(ModelState);
            }

            var result = await _userService.LoginAsync(request);

            return Ok(result);
        }



        [HttpGet("users")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("GetAllUsers endpoint called");

            var result = await _userService.GetAllUsersAsync();

            _logger.LogInformation($"Retrieved {result.Count} users");

            return Ok(result);

        }

        [HttpGet("users/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            _logger.LogInformation($"GetUserById endpoint called with ID: {id}");

            var result = await _userService.GetUserByIdAsync(id);

            _logger.LogInformation($"User retrieved successfully: {id}");

            return Ok(result);

        }

        [HttpDelete("users/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation($"DeleteUser endpoint called with ID: {id}");

            await _userService.DeleteUserAsync(id);

            _logger.LogInformation($"User deleted successfully: {id}");

            return Ok(new
            {
                message = "User deleted successfully.",
                userId = id
            });

        }
    }
}
