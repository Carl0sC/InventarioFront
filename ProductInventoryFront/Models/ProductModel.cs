namespace ProductInventoryFront.Models
{
    public enum ProductState
    {
        Available = 0,
        OutOfStock = 1,
        Defective = 2
    }

    public enum ManufacturingType
    {
        Handmade = 0,
        HandAndMachineMade = 1
    }

    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ManufacturingType ManufacturingType { get; set; }
        public ProductState State { get; set; }
    }
}
