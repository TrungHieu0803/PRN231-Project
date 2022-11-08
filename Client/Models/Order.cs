namespace Client.Models
{
    public class Order
    {
        public string customerId { get; set; } = null!;
        public string? shipAddress { get; set; }
        public List<OrderDetail> orderDetail { get; set; }
    }
}
