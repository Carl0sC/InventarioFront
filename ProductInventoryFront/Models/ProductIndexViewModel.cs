using System.Collections.Generic;

namespace ProductInventoryFront.Models
{
    public class ProductIndexViewModel
    {
        public List<ProductModel> Products { get; set; }
        public int? SelectedState { get; set; }
    }
}
