using Lab7_LeChiCuong_2131200001.Data;
using Lab7_LeChiCuong_2131200001.DTO.User;
using Lab7_LeChiCuong_2131200001.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab7_LeChiCuong_2131200001.Controllers.API
{
    [Authorize(Roles = "Admin")]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<List<User>> GetListUsers()
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserCreationRequest rquest)
        {
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
            return Ok (new { success = false, message = "Fill in all the information" } );
        }

        [HttpPut("{id}")]
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

            return Ok(new { success = false, message = "Fill in all the information" });
        }

        [HttpDelete("{id}")]
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
