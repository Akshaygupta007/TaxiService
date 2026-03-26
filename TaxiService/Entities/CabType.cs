using Microsoft.EntityFrameworkCore;

namespace TaxiService.Entities
{
    public class CabType
    {
        public int CabTypeID { get; set; }
        
        public string CabTypeName { get; set; } = string.Empty;

        public int SeatingCapacity { get; set; }

        [Precision(10, 2)]
        public decimal BaseFare { get; set; }
        [Precision(10, 2)]
        public decimal FarePerKm { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
