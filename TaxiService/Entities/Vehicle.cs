using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiService.Entities
{
    [Table("Vehicles")]
    public class Vehicle
    {
        public int VehicleID { get; set; }

        public string VehicleNumber { get; set; } =string.Empty;

        public string VehicleModel { get; set; } = string.Empty;

        public int ManufactureYear { get; set; }
        public string? Color { get; set; } = string.Empty;

        public VehicleStatus Status { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int CabTypeID { get; set; }
        public CabType? CabType { get; set; }

        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }

    }
    public enum VehicleStatus
    {
        Available,
        Maintenance,
        OutOfService
    }
}
