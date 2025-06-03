using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab7_LeChiCuong_2131200001.Data;
using Lab7_LeChiCuong_2131200001.Models;
using NuGet.Protocol;

namespace Lab7_LeChiCuong_2131200001.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewData["Authors"] = new SelectList(_context.Authors, "AuthorId", "FirstName");
            return PartialView("_CreatePartial");
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
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
                ViewBag.Categories = new MultiSelectList(_context.Categories, "CategoryId", "Name");
                ViewBag.Authors = new MultiSelectList(_context.Authors, "AuthorId", "FirstName");
                return PartialView("_CreatePartial", book);
            }
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Categories)
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(book => book.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            book.SelectedCategoryIds = book.Categories.Select(c => c.CategoryId).ToList();

            book.SelectedAuthorIds = book.Authors.Select(a => a.AuthorId).ToList();

            ViewBag.Categories = new MultiSelectList(_context.Categories, "CategoryId", "Name", book.SelectedCategoryIds);
            ViewBag.Authors = new MultiSelectList(_context.Authors, "AuthorId", "FirstName", book.SelectedAuthorIds);

            return PartialView("_EditPartial", book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/Books/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            Console.WriteLine(book.ToJson());
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

        // GET: Books/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var book = await _context.Books
        //        .FirstOrDefaultAsync(m => m.BookId == id);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    return PartialView("_DeletePartial",book);
        //}

        // POST: Books/Delete/5
        [HttpPost("/Books/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
