using E_Library.Models;
using E_Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace E_Library.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly E_LibraryDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, E_LibraryDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            ViewBag.TotalBooks = _context.Books.Count();
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalCategories = _context.Categories.Count();
            var categories = _context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    BookCount = c.Book.Count()
                });
                         
            ViewBag.CategoriesName = categories.Select(c => c.Name).ToList();
            ViewBag.CategoriesCount = categories.Select(c => c.BookCount).ToList();

            var RecentBooks = _context.Books.Include(c => c.Category)
                .Where(b => !b.IsDeleted && !b.Category.IsDeleted)
                .OrderByDescending(b => b.Id)
                .Take(5).Select(book => new BookFormViewModel
                {
                    Id = book.Id,
                    Name = book.Name,
                    CategoryName = book.Category.Name,
                }).ToList();

            return View(RecentBooks);
        }
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }

                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction("Index", "Account");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("RegisterFailed", error.Description);
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                isPersistent:false,
                lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Account");
                }

                ModelState.AddModelError("LoginFailed", "Invalid Login!");
            }

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        //[HttpGet]
        //public IActionResult AddBooks()
        //{
        //    if (HttpContext.Session.Get("Id") != null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult AddBooks(Book book, IFormFile file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var bookExists = _e_Library.Books.Any(x => x.Name.Equals(book.Name));
        //        if (bookExists)
        //        {
        //            ModelState.AddModelError("Name", "Book already exists!");
        //            return View(book);
        //        }
        //        if(file != null && file.Length > 0)
        //        {

        //        }
        //        _e_Library.SaveChanges();
        //    }

        //    return View();
        //}
    }
}
