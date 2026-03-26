namespace TaxiService.DTOs.Responses
{
    public class DriverVehicleResponse
    {
        public int DriverVehicleID { get; set; }
        public int DriverID { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public int VehicleID { get; set; }
        public string VehicleNumber { get; set; } = string.Empty;
        public DateTime AssignmentDate { get; set; }
        public DateTime? UnassignmentDate { get; set; }
        public bool IsCurrentAssignment { get; set; }
    }
}
