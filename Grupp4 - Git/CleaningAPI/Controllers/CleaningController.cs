using Microsoft.AspNetCore.Mvc;

namespace CleaningAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CleaningController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly CleaningDbContext _context;
        private readonly AuthService _authService;

        private readonly ILogger<CleaningController> _logger;

        public CleaningController(ILogger<CleaningController> logger, CleaningDbContext context, AuthService authService)
        {
            _logger = logger;
            _context = context;
            _authService = authService;
        }

        [HttpGet(Name = "Cleaning")]
        public IEnumerable<CleaningTask> Get()
        {
            var Tasks = _context.CleaningTasks.ToList();

            return Tasks.ToList();
        }
        [HttpPost]
        [Route("AddTask")]
        public async Task<IActionResult> AddTask(CleaningTask Tasks)
        {
            string response = string.Empty;
            _context.CleaningTasks.Add(Tasks);
            _context.SaveChanges();

            return Ok("Task created");
        }

        [HttpPut]
        [Route("UpdateTask")]
        public async Task<IActionResult> UpdateTask(CleaningTask Tasks)
        {
            if (Tasks.Id != 0)
            {
                _context.CleaningTasks.Update(Tasks);
                _context.SaveChanges();
                return Ok("Task updated.");
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
            var TasksInDb = _context.CleaningTasks.SingleOrDefault(s => s.Id == Tasks);

            _context.CleaningTasks.Remove(TasksInDb);
            _context.SaveChanges();
            return Ok("Task Removed");
        }
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "API Running" });
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password)) { 
            return BadRequest("Username or Password is empty or incorrect!");
            }

            var user = await _authService.AuthenticateAsync(loginDto.Username, loginDto.Password);

            if (user == null) {
            return Unauthorized();
            }
            
            return Ok(user);
        }
    }
}
