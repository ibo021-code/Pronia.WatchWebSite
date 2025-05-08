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
           



        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Slide? slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide is null) return NotFound();
            UpdateSlideVM slideVM = new UpdateSlideVM
            {
               
                Title = slide.Title,
                SubTitle = slide.SubTitle,
                Description = slide.Description,
                Order = slide.Order,
                Image = slide.Image,
            };
            return View(slideVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSlideVM slideVM)
        {
            if (!ModelState.IsValid)
            {
                return View(slideVM);
            }
            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();
            if (slideVM.Photo is not null)
            { 
            
                if (slideVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM.Photo), "Please select a valid image file");
                    return View(slideVM);
                }
                if (slideVM.Photo.ValidateSize(FileSize.MB, 1))
                {
                    ModelState.AddModelError(nameof(CreateSlideVM.Photo), "File size must be less than 1MB");
                    return View(slideVM);
                }
               string Filename =await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
                existed.Image = Filename;

            }
            existed.Title = slideVM.Title;
            existed.SubTitle = slideVM.SubTitle;
            existed.Description = slideVM.Description;
            existed.Order = slideVM.Order;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));






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

            //return Content(slide.Photo.FileName+" "+slide.Photo.ContentType+" "+slide.Photo.Length);
            //if (!ModelState.IsValid)  return View();