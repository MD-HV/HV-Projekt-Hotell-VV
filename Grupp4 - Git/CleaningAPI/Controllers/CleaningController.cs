using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CleaningAPI.Models;

namespace CleaningAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CleaningController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly CleaningDbContext _context;
        private readonly ILogger<CleaningController> _logger;

        public CleaningController(ILogger<CleaningController> logger, CleaningDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "Cleaning")]
        public async Task<IEnumerable<CleaningTask>> Get()
        {
            var Tasks = await _context.CleaningTasks.ToListAsync();

            return [.. Tasks];
        }
        [HttpPost]
        [Route("AddTask")]
        public async Task<IActionResult> AddTask(CleaningTask task)
        {
            if(task == null)
            {
                 return BadRequest("Error Task is Empty or Missing!");
            }
            try {
               _context.CleaningTasks.Add(task);
               await _context.SaveChangesAsync();
               return Ok("Task created");
            }
            catch (DbUpdateException dbEx)
            {
                // Log exception (e.g., using a logging framework)
                return StatusCode(500, "Database update error occurred.");
            }
            catch (Exception ex) {
                return StatusCode(500, "An Unexpected Error occured!");
            }
        }

        [HttpPut]
        [Route("UpdateTask")]
        public async Task<IActionResult> UpdateTask(CleaningTask Tasks)
        {
            if (Tasks.Id != 0)
            {
               try{ _context.CleaningTasks.Update(Tasks);
                await _context.SaveChangesAsync();
                return Ok("Task updated.");
                }catch(Exception ex){
                return StatusCode(500, "An Unexpected Error occured!");
                }
            }
            else
            {
                return BadRequest("Error, Task does not exist!");
            }
        }

        [HttpDelete]
        [Route("RemoveTask")]
        public async Task<IActionResult> RemoveTask(int Tasks)
        {
            if (Tasks != 0)
            {
                var TasksInDb = _context.CleaningTasks.SingleOrDefault(s => s.Id == Tasks);
                if (TasksInDb == null)
                {
                    return NotFound("Task not found.");
                }
        
                try
                {
                    _context.CleaningTasks.Remove(TasksInDb);
                    await _context.SaveChangesAsync();
                    return Ok("Task Removed");
                }
                catch (Exception ex)
                {
                    // Optionally log ex here
                    return StatusCode(500, "An Unexpected Error occured!");
                }
            }
        
            return BadRequest("Invalid task ID.");
        }
    }
}
