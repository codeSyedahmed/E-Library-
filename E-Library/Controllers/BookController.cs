using E_Library.Models;
using E_Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_Library.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly E_LibraryDbContext _context;
        public BookController(E_LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> BookList()
        {
            var books = await _context.BookToCategoryMappings
                .Include(b => b.Book).Include(c => c.Category)
                .Where(b => !b.Book.IsDeleted && !b.Category.IsDeleted)
                .OrderBy(b => b.Book.Name).ToListAsync();

            return View(books);
        }
        [HttpGet]
        public IActionResult CreateBook()
        {
            var bookViewModel = new BookFormViewModel();
            bookViewModel.CategoryList = GetCategories();
            return View(bookViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook(BookFormViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                if (bookViewModel.ImageFile != null && bookViewModel.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var uniqueFileName = $"{timestamp}_{bookViewModel.ImageFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await bookViewModel.ImageFile.CopyToAsync(stream);
                    }

                    bookViewModel.Image = $"/uploads/{uniqueFileName}";
                }

                var book = new Book
                {
                    Name = bookViewModel.Name,
                    Description = bookViewModel.Description,
                    Image = bookViewModel.Image

                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                foreach (var categoryId in bookViewModel.SelectedCategoryIds)
                {
                    var mapping = new BookToCategoryMapping
                    {
                        BookId = book.Id,
                        CategoryId = categoryId
                    };
                    _context.BookToCategoryMappings.Add(mapping);
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = "Book added successfully!";
                return RedirectToAction("BookList");
            }

            bookViewModel.CategoryList = GetCategories();
            return View(bookViewModel);
        }

        private List<SelectListItem> GetCategories()
        {
            return _context.Categories.Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToList();
        }
    }
}
