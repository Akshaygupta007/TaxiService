using System.ComponentModel.DataAnnotations;
using TaxiService.Entities;

namespace TaxiService.DTOs.Requests
{
    public class UpdateVehicleRequest
    {
        [Required]
        [StringLength(20)]
        public string? VehicleNumber { get; set; } = string.Empty;

        [StringLength(20)]
        public string? VehicleModel { get; set; } = string.Empty;

        [StringLength(20)]
        public int? ManufactureYear { get; set; }

        [StringLength(100)]
        public string? Color { get; set; } = string.Empty;

        public bool? IsAvailable { get; set; }

        public VehicleStatus? Status { get; set; }
    }
}
