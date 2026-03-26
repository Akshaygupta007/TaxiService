using Microsoft.EntityFrameworkCore;

namespace TaxiService.Entities
{
    public class Driver
    {
        public int DriverID { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string PhoneNumber { get; set; } = string.Empty;
        
        public string LicenseNumber { get; set; } = string.Empty;
        
        public bool IsAvailable { get; set; } = true;

        [Precision(10, 2)]
        public decimal Rating { get; set; } = 0;

        public int TotalRides { get; set; } = 0;

        public int? VehicleId { get; set; }
        
        public Vehicle? Vehicle { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
