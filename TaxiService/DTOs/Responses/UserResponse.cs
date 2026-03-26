namespace TaxiService.DTOs.Responses
{
    public record UserResponse
    {
        public int UserID { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }
    }
}
