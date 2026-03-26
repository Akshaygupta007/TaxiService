namespace TaxiService.DTOs.Responses
{
    public class CabTypeResponse
    {
        public int CabTypeID { get; set; }
        
        public string CabTypeName { get; set; } = string.Empty;

        public int SeatingCapacity { get; set; }
        
        public decimal BaseFare { get; set; }
        
        public decimal PerKmRate { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
