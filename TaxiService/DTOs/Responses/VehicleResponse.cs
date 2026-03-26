using TaxiService.Entities;

namespace TaxiService.DTOs.Responses
{
    public record VehicleResponse
    {
        public int VehicleID { get; set; }

        public string VehicleNumber { get; set; } = string.Empty;

        //public string CabTypeName { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;

        public int ManufacturingYear { get; set; }
        
        public string? Color { get; set; }

        public bool IsActive { get; set; }

        public bool IsAvailable { get; set; }

        public VehicleStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int CabTypeID { get; set; }

        public CabTypeResponse? CabType { get; set; }

    }
}
