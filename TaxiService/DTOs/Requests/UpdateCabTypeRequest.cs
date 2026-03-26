using System.ComponentModel.DataAnnotations;

namespace TaxiService.DTOs.Requests
{
    public class UpdateCabTypeRequest
    {
        [Required]
        [StringLength(50)]
        public string? CabTypeName { get; set; }

        [Range(0, 10000)]
        public decimal? BaseFare { get; set; }

        [Range(0, 1000)]
        public decimal? FarePerKm { get; set; }
    }
}
