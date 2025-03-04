using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<Task> Get()
        {
            var Tasks = _context.Tasks.ToList();

            return Tasks.ToList();
        }
        [HttpPost]
        [Route("AddTask")]
        public string AddTask(Task Tasks)
        {
            string response = string.Empty;
            _context.Tasks.Add(Tasks);
            _context.SaveChanges();

            return "Låt skapad";
        }

        [HttpPut]
        [Route("UpdateTask")]
        public string UpdateTask(Task Tasks)
        {
            if (Tasks.Id != 0)
            {
                _context.Tasks.Update(Tasks);
                _context.SaveChanges();
                return "Låt uppdaterad";
            }
            else
            {
                return "Error, denna låt existerar EJ!";
            }
             
        }

        [HttpDelete]
        [Route("RemoveTask")]
        public string RemoveTask(int Tasks)
        {
            var TasksInDb = _context.Tasks.SingleOrDefault(s => s.Id == Tasks);

            _context.Tasks.Remove(TasksInDb);
            _context.SaveChanges();
            return "Låten har raderats";
             
        }
    }
}
