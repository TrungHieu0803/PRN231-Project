namespace Client.Models
{
    public class CustomerDto
    {
        public string customerId { get; set; } = null!;
        public string companyName { get; set; } = null!;
        public string? contactName { get; set; }
        public string? contactTitle { get; set; }
        public string? address { get; set; }

    }
}
