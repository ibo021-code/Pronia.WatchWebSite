using Watch.website.Models;

namespace Watch.website.ViewModels.Products
{
    public class CreateProductVM
    
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
       
        public List<Category> Categories { get; set;
    }
}
