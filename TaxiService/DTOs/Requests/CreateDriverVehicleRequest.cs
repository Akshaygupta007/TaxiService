using System.ComponentModel.DataAnnotations;

namespace TaxiService.DTOs.Requests
{
    public class CreateDriverVehicleRequest
    {
        [Required(ErrorMessage = "Driver ID is required")]
        [Range(1, int.MaxValue)]
        public int DriverID { get; set; }

        [Required(ErrorMessage = "Vehicle ID is required")]
        [Range(1, int.MaxValue)]
        public int VehicleID { get; set; }

        [DataType(DataType.Date)]
        public DateTime? AssignmentDate { get; set; }
    }
}
