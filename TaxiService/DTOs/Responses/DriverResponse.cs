using TaxiService.Entities;

namespace TaxiService.DTOs.Responses
{
    public record DriverResponse // can be define as DriverDTO
    {
        public int DriverID { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string LicenseNumber { get; set; } = string.Empty;
        
        public string PhoneNumber { get; set; } = string.Empty;

        public int Rating { get; set; }

        public int TotalRides { get; set; }
        
        public bool IsAvailable { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public VehicleResponse? Vehicle { get; set; }
    }
}
