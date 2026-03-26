using System.ComponentModel.DataAnnotations;

namespace TaxiService.Entities
{
    public class DriverVehicle
    {
        public int DriverID { get; set; }
        public int VehicleID { get; set; }

        public bool IsPrimaryDriver { get; set; } = false;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UnassignedDate { get; set; }

        public bool IsCurrentAssignment { get; set; } = true;

        public Driver? Driver { get; set; }
        public Vehicle? Vehicle { get; set; }

    }
}
