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
            var books = await _context.Books
                .Include(c => c.Category)
                .Where(b => !b.IsDeleted && !b.Category.IsDeleted)
                .OrderBy(b => b.Name).Select(book => new BookFormViewModel
                {
                    Id = book.Id,
                    Name = book.Name,
                    Description = book.Description,
                    CategoryName = book.Category.Name,
                    Image = book.Image,
                    PdfFilePath = book.PdfFilePath,
                    IsAvailableOnline = book.IsAvailableOnline,
                    AllowDownload = book.AllowDownload
                }).ToListAsync();

            return View(books);
        }
        [HttpGet]
        public async Task<IActionResult> ReadBook(int id)
        {
            var book = await _context.Books
                .Select(book => new BookFormViewModel
                {
                    Id = book.Id,
                    CategoryId = book.CategoryId,
                    Name = book.Name,
                    Description = book.Description,
                    Image = book.Image,
                    PdfFilePath = book.PdfFilePath,
                    IsAvailableOnline = book.IsAvailableOnline,
                    AllowDownload = book.AllowDownload

                })
                .FirstOrDefaultAsync(b => b.Id == id);
            return View(book);
        }
        [HttpGet]
        public async Task<IActionResult> CreateBook()
        {
            var bookViewModel = new BookFormViewModel();
            bookViewModel.CategoryList = await GetCategories();
            //ViewBag.CategoryList = new SelectList(_context.Categories.Where(c => !c.IsDeleted), "Id", "Name");
            return View(bookViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook(BookFormViewModel bookViewModel)
        {
            bookViewModel.CategoryList = await GetCategories();
            //ModelState.Clear();

            if (bookViewModel.ImageFile == null || bookViewModel.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Image field is required");
            }
            else
            {
                var imageExtension = Path.GetExtension(bookViewModel.ImageFile.FileName).ToLower();
                var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };

                if (!allowedImageExtensions.Contains(imageExtension))
                {
                    ModelState.AddModelError("ImageFile", "Only JPG, JPEG, PNG files are allowed");
                }
            }

            if (bookViewModel.PdfFile == null || bookViewModel.PdfFile.Length == 0)
            {
                ModelState.AddModelError("PdfFile", "PDF file is required");
            }
            else
            {
                var pdfExtension = Path.GetExtension(bookViewModel.PdfFile.FileName).ToLower();
                if (pdfExtension != ".pdf")
                {
                    ModelState.AddModelError("PdfFile", "Only PDF files are allowed");
                }
            }

            try
            {
                var book = new Book
                {
                    Name = bookViewModel.Name,
                    Description = bookViewModel.Description,
                    Image = await FileUpload(bookViewModel.ImageFile),
                    PdfFilePath = await SavePdfFile(bookViewModel.PdfFile),
                    IsAvailableOnline = bookViewModel.IsAvailableOnline,
                    AllowDownload = bookViewModel.AllowDownload,
                    CategoryId = bookViewModel.CategoryId.Value
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Book added successfully!";
                return RedirectToAction("BookList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error saving book: " + ex.Message);
                return View(bookViewModel);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditBook(int id)
        {
            var book = await _context.Books
                .Select(book => new BookFormViewModel
                {
                    Id = book.Id,
                    Name = book.Name,
                    Description = book.Description,
                    Image = book.Image,
                    CategoryId = book.CategoryId

                })
                .FirstOrDefaultAsync(b => b.Id == id);

            book.CategoryList = await GetCategories();

            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(int id, BookFormViewModel bookViewModel)
        {
            bookViewModel.CategoryList = await GetCategories();

            if (bookViewModel.ImageFile != null && bookViewModel.ImageFile.Length > 0)
            {
                var extension = Path.GetExtension(bookViewModel.ImageFile.FileName).ToLower();
                var allowedExtension = new[] { ".jpg", ".jpeg", ".png" };

                if (!allowedExtension.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Only (jpg, jpeg, png) files are allowed!");
                }
            }

            // ✅ PDF validation only if file uploaded
            if (bookViewModel.PdfFile != null && bookViewModel.PdfFile.Length > 0)
            {
                var pdfExtension = Path.GetExtension(bookViewModel.PdfFile.FileName).ToLower();

                if (pdfExtension != ".pdf")
                {
                    ModelState.AddModelError("PdfFile", "Only PDF files are allowed");
                }
            }

            if (ModelState.IsValid)
            {
                var book = await _context.Books
                    .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

                book.Name = bookViewModel.Name;
                book.Description = bookViewModel.Description;
                book.CategoryId = bookViewModel.CategoryId.Value;

                // Image update only if new uploaded
                if (bookViewModel.ImageFile != null)
                {
                    book.Image = await FileUpload(bookViewModel.ImageFile);
                }

                // PDF update only if new uploaded
                if (bookViewModel.PdfFile != null)
                {
                    book.PdfFilePath = await SavePdfFile(bookViewModel.PdfFile);
                }

                _context.Books.Update(book);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Book updated successfully!";
                return RedirectToAction("BookList");
            }

            return View(bookViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var book = await _context.Books.FindAsync(id);
            book.IsDeleted = true;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            TempData["Danger"] = "Book deleted successfully!";
            return RedirectToAction("BookList");

        }

        private async Task<List<SelectListItem>> GetCategories()
        {
            return await _context.Categories.Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToListAsync();
        }

        private async Task<string> FileUpload(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var uniqueFileName = $"{timestamp}_{imageFile.FileName}";

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/{uniqueFileName}";
            
        }

        private async Task<string> SavePdfFile(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "books");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var uniqueFileName = $"{timestamp}_{pdfFile.FileName}";

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            return $"/books/{uniqueFileName}";

        }

    }
}
