namespace Client.Models
{
    public class ProductDto
    {
        public int productId { get; set; }
        public string? productName { get; set; } = null!;
        public int? categoryId { get; set; }
        public string? quantityPerUnit { get; set; }
        public decimal? unitPrice { get; set; }
        public short? unitsInStock { get; set; }
        public short? unitsOnOrder { get; set; }
        public short? reorderLevel { get; set; }
        public bool? discontinued { get; set; }
        public string? categoryName { get; set; }
        public CategoryDto? category { get; set; }

    }
}
