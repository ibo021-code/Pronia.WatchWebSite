using Watch.website.Models;

namespace Watch.website.ViewModels
{
    public class DetailVM
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}
