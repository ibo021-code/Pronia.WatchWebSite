using System.ComponentModel.DataAnnotations;

namespace Watch.website.Models
{
    public class Category : BaseEntity
    {
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
    
    
}
