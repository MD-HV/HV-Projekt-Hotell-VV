using CleaningService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Net.Http;

namespace CleaningService.Controllers
{
    public class CreateController : Controller
    {
        
        public IActionResult Index()
        {
            var task = new CleaningTask();
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(CleaningAPI task)
        {
            
            var Client = new HttpClient();
            var response = await Client.PostAsJsonAsync("https://informatik3.ei.hv.se/CleaningAPI/api/Cleaning/AddTask" , task);
            
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Clean", "Home");
            }

            return View(task);
            
            /*
            {
                
                var apiUrl = "https://informatik3.ei.hv.se/CleaningAPI/api/Cleaning/AddTask";
                var client = new HttpClient();

                var json = JsonConvert.SerializeObject(task); // Serialize data to JSON
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Clean" , "Home"); // Or handle accordingly
                }
                else
                {
                    return RedirectToAction("Index", "Create");
                }
                
            }
            */

        }
    }
}
