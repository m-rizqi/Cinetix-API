using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinetix_Api.Context;
using Cinetix_Api.Models;
using Cinetix_Api.Request;
using Cinetix_Api.Utility;
using Cinetix_Api.Response;
using System.Net;

namespace Cinetix_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return user;
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
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<User>> Login(LoginRequest loginRequest)
        {
            if (!Utility.Utility.IsValidEmail(loginRequest.Email)) {
                return new ErrorResponse<User>(HttpStatusCode.BadRequest, "Email not valid");
            }
            if (!Utility.Utility.IsValidPassword(loginRequest.Password))
            {
                return new ErrorResponse<User>(HttpStatusCode.BadRequest, "Password not valid. It must contain at least a number, an upper case letter, and 8 characters long");
            }
            var user = _context.Users
                .ToList()
                .Where(user => user.Email.Equals(loginRequest.Email) && user.Password.Equals(loginRequest.Password))
                .FirstOrDefault();
            if(user == null)
            {
                return NotFound();
            }
            else
            {
                return user;
            }
        }

        [Route("Register")]
        [HttpPost]
        public async Task<ActionResult<User>> Register(User user)
        {
            if (!Utility.Utility.IsValidEmail(user.Email))
            {
                return new ErrorResponse<User>(HttpStatusCode.BadRequest, "Email not valid");
            }
            if (!Utility.Utility.IsValidPassword(user.Password))
            {
                return new ErrorResponse<User>(HttpStatusCode.BadRequest, "Password not valid. It must contain at least a number, an upper case letter, and 8 characters long");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

    }
}
