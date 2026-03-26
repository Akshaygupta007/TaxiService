using System.ComponentModel.DataAnnotations;

namespace TaxiService.DTOs.Requests
{
    public class CreateCabTypeRequest
    {
        [Required(ErrorMessage = "Cab type name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string CabTypeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Base fare is required")]
        [Range(0, 10000, ErrorMessage = "Base fare must be non-negative")]
        public decimal BaseFare { get; set; }

        [Required(ErrorMessage = "Per KM rate is required")]
        [Range(0, 1000, ErrorMessage = "Per KM rate must be non-negative")]
        public decimal FarePerKm { get; set; }
    }
}
