using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab7_LeChiCuong_2131200001.Data;
using Microsoft.AspNetCore.Components;
using NuGet.Protocol;
using Lab7_LeChiCuong_2131200001.Models;
using Lab7_LeChiCuong_2131200001.DTO.User;
using Azure.Core;

namespace Lab7_LeChiCuong_2131200001.Controllers
{

    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            Console.WriteLine("Inside");
            ViewBag.Roles = new MultiSelectList(_context.Role, "RoleId", "Name");
            return PartialView("_CreatePartial");
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserCreationRequest rquest)
        {
            Console.WriteLine(rquest.ToJson());
            if (ModelState.IsValid)
            {
                var roles = await _context.Role
                    .Where(r => rquest.SelectedRoleIds.Any(t => r.RoleId == t))
                    .ToListAsync();
                var list = new List<UserRole>();
                var user = new User();
                foreach (var role in roles)
                {
                    user.UserRoles.Add(new UserRole()
                    {
                        UserId = user.UserId,
                        RoleId = role.RoleId,
                    });
                }

                user.Fullname = rquest.Fullname;
                user.Email = rquest.Email;
                user.Password = rquest.Password;
                user.UserCode = rquest.UserCode;
                user.Phone = rquest.Phone;
                

                _context.Add(user);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "User created successfully!" });
            }
            return PartialView("_CreatePartial", rquest);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            var updationRequest = new UserUpdationRequest()
            {
                Email = user.Email,
                Password = user.Password,
                Fullname = user.Fullname,
                Phone = user.Phone,
                SelectedRoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList(),
                UserCode = user.UserCode,
            };

            ViewBag.Roles = new MultiSelectList(_context.Role, "RoleId", "Name", updationRequest.SelectedRoleIds);

            return PartialView("_EditPartial", updationRequest);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/Users/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserUpdationRequest request)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users
                        .Include(u => u.UserRoles)
                            .ThenInclude(ur => ur.Role)
                        .FirstAsync(u => u.UserId == id);

                    user.UserCode = request.UserCode;
                    user.Phone = request.Phone;
                    user.Email = request.Email;
                    user.Password = request.Password;
                    user.Fullname = request.Fullname;
                    user.UserCode = request.UserCode;

                    _context.UserRoles.RemoveRange(user.UserRoles);

                    var roles = await _context.Role
                    .Where(r => request.SelectedRoleIds.Any(t => r.RoleId == t))
                    .ToListAsync();
                    var list = new List<UserRole>();
                    foreach (var role in roles)
                    {
                        user.UserRoles.Add(new UserRole
                        {
                            UserId = user.UserId,
                            RoleId = role.RoleId
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(new { success = true, message = "User Updated successfully!" });
            }

            ViewBag.Roles = new MultiSelectList(_context.Role, "RoleId", "Name", request.SelectedRoleIds);
            return PartialView("_EditPartial", request);
        }

        // POST: Users/Delete/5
        [HttpDelete("/Users/Delete/{id}"), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserId == id);
            if (user != null)
            {
                _context.UserRoles.RemoveRange(user.UserRoles);
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
