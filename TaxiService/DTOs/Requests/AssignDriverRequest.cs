using System.ComponentModel.DataAnnotations;

namespace TaxiService.DTOs.Requests
{
    public class AssignDriverRequest
    {
        [Required(ErrorMessage = "Vehicle ID is required")]
        [Range(1, int.MaxValue)]
        public int VehicleID { get; set; }

        [Required(ErrorMessage = "Driver ID is required")]
        [Range(1, int.MaxValue)]
        public int DriverID { get; set; }

        [DataType(DataType.Date)]
        public DateTime? AssignmentDate { get; set; }

    }
}
