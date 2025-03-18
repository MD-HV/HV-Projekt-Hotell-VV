using BookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DinnerTableController : Controller
    {
        private readonly BookingDbContext _context;

        public DinnerTableController(BookingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DinnerTable>>> GetDinnerTables()
        {
            return await _context.DinnerTables.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DinnerTable>> GetDinnerTable(int id)
        {
            var dinnerTable = await _context.DinnerTables.FindAsync(id);

            if (dinnerTable == null)
            {
                return NotFound();
            }

            return dinnerTable;
        }

        [HttpPost]
        public async Task<ActionResult<DinnerTable>> CreateDinnerTable(DinnerTable dinnerTable)
        {
            _context.DinnerTables.Add(dinnerTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDinnerTable), new { id = dinnerTable.TableID }, dinnerTable);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDinnerTable(int id, DinnerTable dinnerTable)
        {
            if (id != dinnerTable.TableID)
            {
                return BadRequest();
            }

            _context.Entry(dinnerTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DinnerTables.Any(e => e.TableID == id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDinnerTable(int id)
        {
            var dinnerTable = await _context.DinnerTables.FindAsync(id);

            if (dinnerTable == null)
            {
                return NotFound();
            }

            _context.DinnerTables.Remove(dinnerTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

