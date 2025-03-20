using BookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Controllers
{
    [Route("api/customer/bookings")]
    [ApiController]
    public class CustomerBookingController : Controller
    {
        private readonly BookingDbContext _context;

        public CustomerBookingController(BookingDbContext context)
        {
            _context = context;
        }

        // Hämta alla tillgängliga bord
        [HttpGet("tables")]
        public async Task<ActionResult<IEnumerable<DinnerTable>>> GetAvailableTables()
        {
            var tables = await _context.DinnerTables.ToListAsync();
            return Ok(tables);
        }

        // Lägg till en bokning
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            // Kontrollera om bordet finns
            var tableExists = await _context.DinnerTables.AnyAsync(t => t.TableID == booking.TableID_FK);
            if (!tableExists)
            {
                return BadRequest("Det angivna bordet finns inte.");
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingID }, booking);
        }

        // Hämta en specifik bokning
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.Include(b => b.DinnerTable)
                                                 .FirstOrDefaultAsync(b => b.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }
    }
}

