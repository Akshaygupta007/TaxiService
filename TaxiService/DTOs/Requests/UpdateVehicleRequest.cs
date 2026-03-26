using System.ComponentModel.DataAnnotations;

namespace TaxiService.DTOs.Requests
{
    public class UpdateVehicleRequest
    {

        [StringLength(20)]
        public string? VehicleNumber { get; set; } = string.Empty;

        [StringLength(20)]
        public string? VehicleModel { get; set; } = string.Empty;

        [StringLength(20)]
        public int? ManufactureYear { get; set; }

        [StringLength(100)]
        public string? Color { get; set; } = string.Empty;
    }
}
