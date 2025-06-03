using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Lab7_LeChiCuong_2131200001.Data;
using Lab7_LeChiCuong_2131200001.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace Lab7_LeChiCuong_2131200001.Controllers
{
    [Route("Book")]
    public class BookController : Controller
    {

        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _env;

        public BookController(AppDbContext context, IWebHostEnvironment env)
        {

            _context = context;
            _env = env;

        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string bookCode)
        {
            var book = await _context.Books
                .Include(book => book.Categories)
                .FirstOrDefaultAsync(book => book.BookCode == bookCode);
            return View(book);
        }

        [HttpGet("PageProgrammingBook")]
        public async Task<IActionResult> PageProgrammingBook()
        {

            var books = await _context.Books
                  .Include(category => category.Categories)
                  .Where(book => book.Categories.Any(cate => cate.Name == "Programming"))
               .ToListAsync();
            return View(books);
        }

        [HttpGet("pageFictionBook")]
        public async Task<IActionResult> PageFictionBook()
        {

            var books = await _context.Books
                  .Include(category => category.Categories)
                  .Where(book => book.Categories.Any(cate => cate.Name == "Fiction"))
               .ToListAsync();
            return View(books);
        }

        [Authorize]
        [HttpGet("ViewPDF")]
        public async Task<IActionResult> ViewPDF([FromQuery] string bookCode)
        {

            var book = await _context.Books
                .FirstOrDefaultAsync(book => book.BookCode == bookCode);

            if (book == null)
            {
                Console.WriteLine(bookCode);
                ViewBag.Error = true;
                ViewBag.ErrorMessage = "PDF path does not exist";
            }
            else
            {
                if (!System.IO.File.Exists(_env.WebRootPath + "/assets/pdf/" + book.Pdf))
                {
                    ViewBag.Error = true;
                    ViewBag.ErrorMessage = "PDF path does not exist";
                } else
                {
                    ViewBag.Error = false;
                }
            }

            return View(book);
        }

        
        [HttpGet("getPdf/{fileName}")]
        public IActionResult GetPdf(string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, "assets/pdf", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            Response.Headers["Content-Disposition"] = "inline; filename={fileName}";
            return File(fileBytes, "application/pdf");
        }

        [HttpGet("getBookById/{id:int}")]
        public async Task<JsonResult> GetBookByID([FromRoute] int id)
        {
            var book = await _context.Books
                .Include(book => book.Authors)
                .Include(book => book.Categories)
                .FirstOrDefaultAsync(book => book.BookId == id);
            return Json(book, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
        }



    }
}
