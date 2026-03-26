namespace TaxiService.Entities
{
    public class Driver
    {
        public int DriverID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<DriverVehicle> Vehicles { get; set; } = new List<DriverVehicle>(); //one driver can have many vehicles
    }
}
