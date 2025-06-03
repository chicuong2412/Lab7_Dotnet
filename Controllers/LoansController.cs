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
    public class LoansController : Controller
    {
        private readonly AppDbContext _context;

        public LoansController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Loans

        // GET: Loans/Create
        public IActionResult Create()
        {
            ViewBag.BookId = new SelectList(_context.Books, "BookId", "BookCode");
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "UserCode");
            return PartialView("_CreatePartial");
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Loan loan)
        {
            loan.Book = await _context.Books.FindAsync(loan.BookId);
            loan.User = await _context.Users.FindAsync(loan.UserId);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(loan);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Success");
                    return Ok(new { success = true });
                }
                catch (Exception _ex)
                {
                }
            }
            ViewBag.BookId = new SelectList(_context.Books, "BookId", "BookCode", loan.BookId);
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "Email", loan.UserId);
            return PartialView("_CreatePartial", loan);
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewBag.BookId = new SelectList(_context.Books, "BookId", "BookCode", loan.BookId);
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "Email", loan.UserId);
            return PartialView("_EditPartial", loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Loan loan)
        {
            if (id != loan.LoanId)
            {
                return NotFound();
            }

            loan.Book = await _context.Books.FindAsync(loan.BookId);
            loan.User = await _context.Users.FindAsync(loan.UserId);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.LoanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(new { success = true });
            }
            ViewBag.BookId = new SelectList(_context.Books, "BookId", "BookCode", loan.BookId);
            ViewBag.UserId = new SelectList(_context.Users, "UserId", "Email", loan.UserId);
            return PartialView("_EditPartial", loan);
        }

        // POST: Loans/Delete/5
        [HttpDelete("/Loans/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
            }

            await _context.SaveChangesAsync();
            return Ok(new {success = true});
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.LoanId == id);
        }
    }
}
