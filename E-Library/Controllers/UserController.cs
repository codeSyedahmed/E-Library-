using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Library.Controllers
{
    public class UserController : Controller
    {
        public UserController()
        {
        }
        public IActionResult Index()
        {
            //if (User.IsInRole("Admin"))
            //{
            //    return RedirectToAction("Index");
            //}
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> Register()
        //{
        //    // Pass categories to the view
        //    //ViewData["Roles"] = roles.Result.Data;
        //    return View();
        //}
       // [HttpPost]
        //public async Task<IActionResult> Register()
        //{
        //    //if (result.IsSuccess)
        //    //    ViewBag.SuccessMessage = result.Message;
        //    //else
        //    //    ViewBag.ErrorMessage = result.Message;
        //    return View();
        //}
        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Login(AuthenticationRequestDto request)
        //{
           
        //        var result = await _userService.LoginAsync(request);
        //        if (result.IsSuccess == 1)
        //        {
        //            var claims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Name, request.Username)
        //                // Add other claims as needed
        //            };

        //            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //            var authProperties = new AuthenticationProperties
        //            {
        //                IsPersistent = true, // Remember me functionality
        //                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20) // Cookie expiration time
        //                                                                  // Configure other properties as needed
        //            };

        //            await HttpContext.SignInAsync(
        //                CookieAuthenticationDefaults.AuthenticationScheme,
        //                new ClaimsPrincipal(claimsIdentity),
        //                authProperties);
        //            TempData["success"] = result.Message;
        //            return RedirectToAction("Index", "User");
        //        }
        //        else
        //        {
        //            ViewBag.ErrorMessage = result.Message;
        //            return RedirectToAction("Login");
        //        }
              
        //}
        //public async Task<IActionResult> Logout()
        //{   
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    //TempData["success"] = result.Message;
        //    return RedirectToAction("Login", "User");
      
        //}
    }
}
