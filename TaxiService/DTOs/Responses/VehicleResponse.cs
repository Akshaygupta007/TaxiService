using TaxiService.Entities;

namespace TaxiService.DTOs.Responses
{
    public record VehicleResponse
    {
        public int VehicleID { get; set; }
        public string VehicleNumber { get; set; } = string.Empty;
        public int CabTypeID { get; set; }

        public CabType? CabType { get; set; }
        //public string CabTypeName { get; set; } = string.Empty;
        public int ManufacturingYear { get; set; }
        public string? Color { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<DriverVehicle>? AssignedDrivers { get; set; }
    }
}
