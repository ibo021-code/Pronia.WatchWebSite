using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Watch.website.DAL;
using Watch.website.Models;
using Watch.website.Utilities.Enums;
using Watch.website.Utilities.Extensions;
using Watch.website.ViewModels;

namespace Watch.website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SlideController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetSlideVM> slideVMs = await _context.Slides.Select(s=>
            
                new GetSlideVM
                {
                    Id = s.Id,
                    Title = s.Title,
                    Image = s.Image,
                    CreatedAt = s.CreatedAt,
                    Order = s.Order,

                }
            ).ToListAsync();

            //List<GetSlideVM> slideVMs = new List<GetSlideVM>();
            //foreach (Slide slide in slides)
            //{
            //   slideVMs.Add(new GetSlideVM)
            //    {
            //        CreatedAt = slide.CreatedAt,
            //        Title = slide.Title,
            //        Image = slide.Image,
            //        Id = slide.Id,
            //        Order = slide.Order,
            //    };
            //}
            return View(slideVMs);
        }

        //public  string Test()
        //{
            
        //    return Guid.NewGuid().ToString();
        //}
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {




            if (!slideVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Photo), "Please select a valid image file");
                return View();

            }
            if (!slideVM.Photo.ValidateSize(FileSize.MB,1))
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Photo), "File size must be less than 1MB");
                return View();
            }

            string filename = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");

            Slide slide = new Slide
            {
                Title = slideVM.Title,
                SubTitle = slideVM.SubTitle,
                Description = slideVM.Description,
                Order = slideVM.Order,
                Image = filename,
                CreatedAt = DateTime.Now,
            };     
            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
           


            //return Content(slide.Photo.FileName+" "+slide.Photo.ContentType+" "+slide.Photo.Length);







            //if (!ModelState.IsValid)  return View();



        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || id<=0) return BadRequest();
            
            Slide? slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (slide is null) return NotFound();
            slide.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
            _context.Remove(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         
            
        }
    }
}
