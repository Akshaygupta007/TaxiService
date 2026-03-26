using Microsoft.EntityFrameworkCore;

namespace TaxiService.Entities
{
    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        PayPal,
        Cash
    }
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }
    public class Payment
    {
        public int PaymentID { get; set; }
        public int BookingID { get; set; }
        [Precision(10, 2)]
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }

        public PaymentStatus Status { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
