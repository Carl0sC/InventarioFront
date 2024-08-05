namespace ProductInventoryFront.Models
{
    public class CreateProductModel
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
    }
}
