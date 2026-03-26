using TaxiService.Services.Interfaces;
using TaxiService.Repositories.Interfaces;
using System.Text;
using System.Security.Cryptography;
using TaxiService.DTOs.Responses;
using TaxiService.DTOs.Requests;
using AutoMapper;
using TaxiService.Entities;


namespace TaxiService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            // Validate request
            if (request == null)
            {
                _logger.LogWarning("Login attempt with null request");
                throw new ArgumentNullException(nameof(request), "Login request cannot be null");
            }
            // Validate required fields
            ValidateLoginRequest(request);

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning($"Login attempt with non-existent or inactive email: {request.Email}");
                throw new UnauthorizedAccessException("Invalid email or password");
            }
            // Verify password
            var isPasswordValid = VerifyPassword(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                _logger.LogWarning($"Login attempt with incorrect password for email: {request.Email}");
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            _logger.LogInformation($"User logged in successfully with ID: {user.UserID}");

            // Map and return response with token
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            // Validate request
            if (request == null)
            {
                _logger.LogWarning("Register attempt with null request");
                throw new ArgumentNullException(nameof(request), "Registration request cannot be null");
            }
            // Validate required fields
            ValidateRegisterRequest(request);

            // Check if email already exists
            var userExists = await _userRepository.GetByEmailAsync(request.Email);
            if (userExists != null)
            {
                _logger.LogWarning($"Registration attempt with existing email: {request.Email}");
                throw new InvalidOperationException($"Email '{request.Email}' is already registered");
            }

            // Create new user
            var user = new User
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLower(),
                PasswordHash = HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber.Trim(),
                CreatedAt = DateTime.UtcNow,
            };

            // Add user to repository
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation($"User registered successfully with ID: {user.UserID}");

            // Map and return response
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<List<UserResponse>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all users");

            var users = await _userRepository.GetAllAsync();

            if (users == null || users.Count == 0)
            {
                _logger.LogInformation("No users found in system");
                return new List<UserResponse>();
            }

            _logger.LogInformation($"Retrieved {users.Count} users successfully");
            return _mapper.Map<List<UserResponse>>(users);
        }

        public async Task<UserResponse> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0", nameof(userId));
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User not found with ID: {userId}");
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            _logger.LogInformation($"User retrieved successfully: {userId}");
            return _mapper.Map<UserResponse>(user);

        }

        public async Task DeleteUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0", nameof(userId));
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"Delete attempt for non-existent user: {userId}");
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();
            _logger.LogInformation($"User deleted successfully: {userId}");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        /// <summary>
        /// Validate register request
        /// </summary>
        private void ValidateRegisterRequest(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name is required", nameof(request.Name));

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required", nameof(request.Email));

            if (!IsValidEmail(request.Email))
                throw new ArgumentException("Email format is invalid", nameof(request.Email));

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required", nameof(request.Password));

            if (request.Password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters", nameof(request.Password));

            if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
                throw new ArgumentException("Confirm password is required", nameof(request.ConfirmPassword));

            if (request.Password != request.ConfirmPassword)
                throw new ArgumentException("Passwords do not match", nameof(request.ConfirmPassword));
        }

        private void ValidateLoginRequest(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required", nameof(request.Email));

            if (!IsValidEmail(request.Email))
                throw new ArgumentException("Email format is invalid", nameof(request.Email));

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required", nameof(request.Password));
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            // Hash the input password and compare with stored hash
            var hashedInput = HashPassword(password);
            return hashedInput == passwordHash;
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
