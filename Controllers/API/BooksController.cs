using Lab7_LeChiCuong_2131200001.Data;
using Lab7_LeChiCuong_2131200001.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace Lab7_LeChiCuong_2131200001.Controllers.API
{
    [Authorize(Roles = "Admin")]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Book>> GetListBooks()
        {
            return await _context.Books
                .Include(book => book.Authors)
                .Include(book => book.Categories)
                .ToListAsync();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook([FromForm] Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.SelectedCategoryIds != null)
                {
                    book.Categories = _context.Categories.Where(c => book.SelectedAuthorIds.Contains(c.CategoryId)).ToList();
                }

                if (book.SelectedAuthorIds != null)
                {
                    book.Authors = _context.Authors.Where(c => book.SelectedAuthorIds.Contains(c.AuthorId)).ToList();
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Book created successfully!" });
            }
            else
            {
                return Ok(new { success = false, message = "Please fill in all the information" });
            }
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] Book book)
        {
            var bookInDb = await _context.Books
                .Include(b => b.Categories)
                .Include(Book => Book.Authors)
                .FirstOrDefaultAsync(b => b.BookId == id);
            if (bookInDb == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bookInDb.Categories.Clear();
                    bookInDb.Categories = _context.Categories.Where(c => book.SelectedCategoryIds.Contains(c.CategoryId)).ToList();
                    bookInDb.Title = book.Title;
                    bookInDb.Description = book.Description;
                    bookInDb.BookCode = book.BookCode;
                    bookInDb.Publisher = book.Publisher;
                    bookInDb.TotalCopies = book.TotalCopies;
                    bookInDb.AvailableCopies = book.AvailableCopies;
                    bookInDb.Avatar = book.Avatar;
                    bookInDb.Pdf = book.Pdf;
                    bookInDb.Authors.Clear();
                    bookInDb.Authors = _context.Authors.Where(c => book.SelectedAuthorIds.Contains(c.AuthorId)).ToList();

                    _context.Update(bookInDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return Ok(new { success = true, message = "Model state is valid." });
            }
            else
            {
                return Ok(new { success = false, message = "Model state is invalid." });
            }
        }

        [HttpDelete("{id}")]
        public async Task DeleteBook([FromRoute] int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
        }
    }
}
