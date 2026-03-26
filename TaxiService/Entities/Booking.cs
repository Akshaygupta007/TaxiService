using System.ComponentModel.DataAnnotations;
namespace TaxiService.Entities
{
    public enum BookingStatus
    {
        PendingPayment,
        Confirmed,
        Cancelled,
        Completed
    }
    public class Booking
    {
        public int BookingID { get; set; }

        public int UserID { get; set; }
        public int DriverID { get; set; }
        public int VehicleID { get; set; }

        public string PickUpLocation { get; set; } = string.Empty;

        public string DropOffLocation { get; set; } = string.Empty;

        public DateTime PickupTime { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
