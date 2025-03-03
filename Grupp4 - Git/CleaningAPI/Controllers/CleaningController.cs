using Microsoft.AspNetCore.Mvc;

namespace CleaningAPI.Controllers
{
    public class CleaningController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    /* 
    Referenser till tidigare kod för API, fixa problemen och ändra namnen till det som ska finnas i denna API!


    [Route("api/[controller]")]
    [ApiController]
    public class DoodilyController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly APIDbContext _context;

        private readonly ILogger<DoodilyController> _logger;

        public DoodilyController(ILogger<DoodilyController> logger, APIDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "Doodily")]
        public IEnumerable<Song> Get()
        {
            var songs = _context.Songs.ToList();

            return songs.ToList();
        }
        [HttpPost]
        [Route("AddSong")]
        public string AddSong(Song songs)
        {
            string response = string.Empty;
            _context.Songs.Add(songs);
            _context.SaveChanges();

            return "Låt skapad";
        }

        [HttpPut]
        [Route("UpdateSong")]
        public string UpdateSong(Song songs)
        {
            if (songs.Id != 0)
            {
                _context.Songs.Update(songs);
                _context.SaveChanges();
                return "Låt uppdaterad";
            }
            else
            {
                return "Error, denna låt existerar EJ!";Eftersom ingen data hämtas så skickas ett meddelande iväg.
            }

            
             Om detta inte fungerar kan man testa:
            _context.Entry(songs).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
             
        }

        [HttpDelete]
        [Route("RemoveSong")]
        public string RemoveSong(int songs)
        {
            var songsInDb = _context.Songs.SingleOrDefault(s => s.Id == songs);

            _context.Songs.Remove(songsInDb);
            _context.SaveChanges();
            return "Låten har raderats";

             Tror att detta är ganska overkill och kräver att all info om låten i databasen. 
             * bytte från RemoveSong(Song songs) till RemoveSong(int songs)
             * Om denna version ej fungerar så testar vi:
             * _context.Songs.Remove(songs);
             * _context.SaveChanges();
             
        }
    }*/
}
