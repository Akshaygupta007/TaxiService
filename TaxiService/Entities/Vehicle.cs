namespace TaxiService.Entities
{
    public class Vehicle
    {
        public int VehicleID { get; set; }

        public int CabTypeID { get; set; }
        public CabType? CabType { get; set; }

        public string VehicleNumber { get; set; } =string.Empty;

        public string VehicleModel { get; set; } = string.Empty;

        public int ManufactureYear { get; set; }
        public string? Color { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<DriverVehicle> Drivers { get; set; } = new List<DriverVehicle>(); //one Vehicle can have many drivers

    }
}
