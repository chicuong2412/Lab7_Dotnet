using System.Security.Claims;
using Lab7_LeChiCuong_2131200001.Data;
using Lab7_LeChiCuong_2131200001.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Lab7_LeChiCuong_2131200001.Controllers
{
    public class AccountController : Controller
    {

        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( LoginModel rq)
        {
            Console.WriteLine("Email: " + rq.Email);
            Console.WriteLine(rq.Email + " " + rq.Password);
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == rq.Email);

            if (user == null || !user.Password.Equals(rq.Password))
            {
                ModelState.AddModelError("", "Email or password is not correct");
                return View();
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserCode)
            };

            claims.AddRange(user.UserRoles.Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name)));
            var claimIdentity = new ClaimsIdentity(claims, "CookiesAuth");

            var claimPrincipal = new ClaimsPrincipal(claimIdentity);

            await HttpContext.SignInAsync("CookiesAuth", claimPrincipal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookiesAuth");

            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}
