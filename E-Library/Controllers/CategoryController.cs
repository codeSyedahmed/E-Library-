using E_Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Library.Controllers
{
    public class CategoryController : Controller
    {
        private readonly E_LibraryDbContext _context;

        public CategoryController(E_LibraryDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> CategoryList()
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted).OrderBy(c => c.Name).ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                bool exists = await _context.Categories
                    .AnyAsync(c => c.Name.ToLower() == category.Name.ToLower());

                if (exists)
                {
                    ModelState.AddModelError("Name", "Category already exists");
                    return View(category);
                }

                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Category added successfully!";
                return RedirectToAction("CategoryList");
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int? id)
        {
            
            var categories = await _context.Categories.FindAsync(id);
            
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, Category category)
        {
            if (id == category.Id)
            {
                if (ModelState.IsValid)
                {
                    bool exists = await _context.Categories.Where(c => !c.IsDeleted)
                        .AnyAsync(c => c.Name.ToLower() == category.Name.ToLower() && c.Id != category.Id);

                    if (exists)
                    {
                        ModelState.AddModelError("Name", "Category already exists");
                        return View(category);
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Category updated successfully!";
                    return RedirectToAction("CategoryList");
                }
            }
            return View(category);
        }

        //[HttpGet]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    var categories = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        //    return View(categories);
        //}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            
            var category = await _context.Categories.FindAsync(id);
            category.IsDeleted = true;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            TempData["Danger"] = "Category deleted successfully!";
            return RedirectToAction("CategoryList");
            
            
        }
    }
}
