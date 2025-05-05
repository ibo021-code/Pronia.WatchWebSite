﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using Watch.website.DAL;
using Watch.website.Models;

namespace Watch.website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.Include(c => c.Products).AsNoTracking().ToListAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }

            bool result = await _context.Categories.AnyAsync(c => c.Name == category.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(Category.Name), "This category already exists");
                return View();
            }



            category.CreatedAt = DateTime.Now;


            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) BadRequest();
            
                Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (category is null) return NotFound();

                return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Category category)
        {
          if(!ModelState.IsValid) return View();

            bool result = await _context.Categories.AnyAsync(c => c.Name == category.Name && c.Id!=id);
            if (result)
            {
                ModelState.AddModelError(nameof(Category.Name), "This category already exists");
                return View();
            }
            Category? existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);



            if (existed.Name== category.Name) return RedirectToAction(nameof(Index));




            existed.Name = category.Name;
            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }


    }
}


        
        
        

    


