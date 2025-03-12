using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MonitoringGUI.Models;
using Newtonsoft.Json;

namespace MonitoringGUI.Controllers
{
    [Route("Monitoring")]
    public class MonitoringController : Controller
    {
        private readonly HttpClient _httpClient;

        [HttpGet("Index")]
        public IActionResult Index()
        {
            var sessionId = Request.Cookies["SessionID"];

            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public MonitoringController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Health()
        {
            var sessionId = Request.Cookies["SessionID"];

            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _httpClient.GetAsync("https://localhost:7109/api/monitoring/health");
            var content = await response.Content.ReadAsStringAsync();

            var apiStatuses = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            ViewBag.ApiStatuses = apiStatuses;

            return View();
        }


        [HttpGet("Stats")]
        public async Task<IActionResult> Stats()
        {
            var sessionId = Request.Cookies["SessionID"];
            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _httpClient.GetAsync("https://localhost:7109/api/monitoring/stats");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"API Response från MonitoringService: {content}");

            var apiStats = JsonConvert.DeserializeObject<Dictionary<string, List<dynamic>>>(content);

            if (apiStats == null || apiStats.Count == 0)
            {
                Console.WriteLine("🚨 Ingen statistik hämtades från API:et!");
                apiStats = new Dictionary<string, List<dynamic>>();
            }

            ViewBag.ApiStats = apiStats;

            return View();
        }




    }

}
