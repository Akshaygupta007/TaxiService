using System.ComponentModel.DataAnnotations;

namespace TaxiService.DTOs.Requests
{
    public class UpdateDriverRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(20, MinimumLength = 10,
            ErrorMessage = "Phone number must be 10 characters")]
        public string? PhoneNumber { get; set; }
    }
}
