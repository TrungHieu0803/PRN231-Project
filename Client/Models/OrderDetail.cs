namespace Client.Models
{
    public class OrderDetail
    {
        public int productId { get; set; }
        public int quantity { get; set; }

        public string? productName { get; set; }
        public decimal? unitPrice { get; set; }

    }
}
