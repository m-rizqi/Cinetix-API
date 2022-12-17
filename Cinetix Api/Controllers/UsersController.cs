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

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
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

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
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

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
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
        public IActionResult Login(LoginRequest LoginRequest)
        {
            if (!Utility.Utility.IsValidEmail(LoginRequest.Email)) {
                return new ErrorResponse(HttpStatusCode.BadRequest, "Email not valid");
            }
            if (!Utility.Utility.isValidPassword(LoginRequest.Password))
            {
                return new ErrorResponse(HttpStatusCode.BadRequest, "Password not valid. It must contain at least a number, an upper case letter, and 8 characters long");
            }
            var user = _context.Users
                .ToList()
                .Where(user => user.Email.Equals(LoginRequest.Email) && user.Password.Equals(LoginRequest.Password))
                .FirstOrDefault();
            if(user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }
        }

        [Route("Register")]
        [HttpPost]
        public async Task<ActionResult<User>> Register(User user)
        {
            if (user.Email.Length == 0)
            {

            }
            if (user.Password.Length < 8)
            {

            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

    }
}
