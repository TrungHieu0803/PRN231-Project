namespace ProjectPRN231.Dtos
{
    public class ProductDto
    {
        public string ProductName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public string? CategoryName { get; set; } = null;
        public bool Discontinued { get; set; }
    }
}
