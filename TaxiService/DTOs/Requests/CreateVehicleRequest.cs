using System.ComponentModel.DataAnnotations;
namespace TaxiService.DTOs.Requests
{
    public class CreateVehicleRequest
    {
        [Required(ErrorMessage = "Cab type ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid cab type")]
        public int CabTypeID { get; set; }

        [Required(ErrorMessage = "Vehicle number is required")]
        [StringLength(20, MinimumLength = 5)]
        public string VehicleNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string VehicleModel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Manufacturing year is required")]
        [Range(1900, 2050, ErrorMessage = "Invalid year")]
        public int ManufactureYear { get; set; }

        [StringLength(100)]
        public string? Color { get; set; } = string.Empty;
    }
}
