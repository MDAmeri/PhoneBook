using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Data;
using PhoneBook.DTOs;
using PhoneBook.Models;

namespace PhoneBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Only authenticated users can access this controller
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/contacts
        // Supports optional filtering by name or mobile number
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts(
            [FromQuery] string? search = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var query = _context.Contacts
                .Where(c => c.UserId == userId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(c =>
                    c.FullName.ToLower().Contains(search) ||
                    (c.MobileNumber != null && c.MobileNumber.Contains(search)) ||
                    (c.HomeNumber != null && c.HomeNumber.Contains(search)));
            }

            var contacts = await query
                .OrderBy(c => c.FullName)
                .Select(c => new ContactDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    MobileNumber = c.MobileNumber,
                    HomeNumber = c.HomeNumber,
                    Notes = c.Notes
                })
                .ToListAsync();

            return Ok(contacts);
        }

        // GET: api/contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDto>> GetContact(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub");

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (contact == null)
                return NotFound();

            return Ok(new ContactDto
            {
                Id = contact.Id,
                FullName = contact.FullName,
                MobileNumber = contact.MobileNumber,
                HomeNumber = contact.HomeNumber,
                Notes = contact.Notes
            });
        }

        // POST: api/contacts
        [HttpPost]
        public async Task<ActionResult<ContactDto>> CreateContact(ContactDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var contact = new Contact
            {
                FullName = dto.FullName,
                MobileNumber = dto.MobileNumber,
                HomeNumber = dto.HomeNumber,
                Notes = dto.Notes,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            dto.Id = contact.Id;

            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, dto);
        }

        // PUT: api/contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, ContactDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub");

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (contact == null)
                return NotFound();

            contact.FullName = dto.FullName;
            contact.MobileNumber = dto.MobileNumber;
            contact.HomeNumber = dto.HomeNumber;
            contact.Notes = dto.Notes;
            contact.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub");

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (contact == null)
                return NotFound();

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}