using Microsoft.AspNetCore.Mvc;
using Watch.website.DAL;
using Watch.website.ViewModels;
using Watch.website.Models;
using Microsoft.EntityFrameworkCore;

namespace Watch.website.Controllers
{
    public class HomeController : Controller
    {
        public  readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult>  Index()
        {

            

            HomeVM homeVM = new HomeVM()
            {
                Slides =await _context
                .Slides
                .OrderBy(s => s.Order)
                .ToListAsync(),
                Products =await _context.Products
                .Include(p=>p.ProductImages.Where(pi => pi.IsPrimary!=null))
                .ToListAsync()
            };
           
           
            return View(homeVM);

        } 
    } 
}