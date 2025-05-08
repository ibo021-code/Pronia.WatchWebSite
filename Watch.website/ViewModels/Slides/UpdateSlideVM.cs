using System.ComponentModel.DataAnnotations;
using Watch.website.Models;

namespace Watch.website.ViewModels
{
    public class UpdateSlideVM
    {
        [MaxLength(200, ErrorMessage = "Slide Title must be less than 200 characters")]

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
