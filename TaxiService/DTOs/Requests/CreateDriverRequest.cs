using System.ComponentModel.DataAnnotations;

namespace TaxiService.DTOs.Requests
{
    public class CreateDriverRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$",
            ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "License number is required")]
        [StringLength(20, MinimumLength = 5,
            ErrorMessage = "License number must be between 5 and 20 characters")]
        public string LicenseNumber { get; set; } = string.Empty;
    }
}
