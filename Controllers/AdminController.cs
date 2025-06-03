using Lab7_LeChiCuong_2131200001.Data;
using Lab7_LeChiCuong_2131200001.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab7_LeChiCuong_2131200001.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _env;

        public AdminController(AppDbContext context, IWebHostEnvironment env)
        {

            _context = context;
            _env = env;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserTable()
        {
            var list = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> BookManagement()
        {
            var books = await _context.Books
                  .Include(category => category.Categories)
                   .Include(author => author.Authors)
               .ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> CategoryManagement()
        {

            return View(await _context.Categories.ToListAsync());
        }

        public async Task<IActionResult> AuthorManagement()
        {
            return View(await _context.Authors.ToListAsync());
        }

        public async Task<IActionResult> LoanManagement()
        {
            return View(await _context.Loans
                .Include(a => a.User)
                .Include(a => a.Book)
                .ToListAsync());
        }
    }
}
