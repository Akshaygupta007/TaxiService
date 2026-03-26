using System.ComponentModel.DataAnnotations;

namespace TaxiService.DTOs.Requests
{
    public class UnassignDriverRequest
    {
        [Required(ErrorMessage = "Vehicle ID is required")]
        [Range(1, int.MaxValue)]
        public int VehicleID { get; set; }
    }
}
