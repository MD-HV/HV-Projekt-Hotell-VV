using System.Diagnostics;
using CleaningService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace CleaningService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CleaningDbContext _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //----------------------------------------------------------------------------

        [Authorize]
        public async Task<IActionResult> Clean()
        {
            List<CleaningAPI>? tasksAPI = null;

            var client = new HttpClient();

            // Creates a http request and changes the URI to the correct localhost.
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://informatik3.ei.hv.se/CleaningAPI/api/Cleaning"),
            };

            try
            {
                // Sends request to the API and puts the json response to the List
                using (var response = await client.SendAsync(request))
                {
                    tasksAPI = await response.Content.ReadFromJsonAsync<List<CleaningAPI>>();
                }
            }
            catch (Exception e)
            {
                // Gives an error if the API is not available
                _logger.LogError(e.Message);
            }

            // Returns the result to the View for rendering
            return View(tasksAPI);
        }

        //-----------------------------------------------------------------------------

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
