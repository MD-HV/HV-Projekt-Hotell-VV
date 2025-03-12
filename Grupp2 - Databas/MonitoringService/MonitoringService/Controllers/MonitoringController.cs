using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using MonitoringService.Models;
using Newtonsoft.Json;

namespace MonitoringService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Threading.Tasks;

    [Route("api/monitoring")]//rot-URL
    [ApiController]
    public class MonitoringController : Controller
    {
        private static int _requestCount = 0; //statisk variabel som håller koll på hur många gånger "health"-endpointen har anropats
        private readonly HttpClient _httpClient; //används för att skicka HTTP-anrop till API:n

        public MonitoringController(HttpClient httpClient)//konstruktor som gör det möjligt att skicka andra HTTP-förfrågningar till andra API:er
        {
            _httpClient = httpClient;
        }

        [HttpGet("health")]//endpoint för att kolla status på loginserve
        public async Task<IActionResult> CheckLoginApiHealth()//anrop sker asynkront
        {
            _requestCount++; //ökar räknaren för antal anrop, just nu för antal av denna, men vi tänkte ha en som räknar hur många gånger det andra API:et anropats, men det blir senare

            var apis = new Dictionary<string, string>//dictionary med API:er och deras hälso-check-URL:er
            {
                { "LoginService", "https://localhost:7130/api/auth/login/health" },
                { "RoomService", "https://localhost:7200/api/user/health" },
                { "RestaurantService", "https://localhost:7300/api/booking/health" },
                { "CleaningService", "https://localhost:7300/api/booking/health" },
                { "BookingService", "https://localhost:7300/api/booking/health" }
            };
            var apiStatuses = new Dictionary<string, string>();//dictionary för att lagra status på API:erna

            foreach (var api in apis)//för varje API i listan
            {
                try
                {
                    var response = await new HttpClient().GetAsync(api.Value);//anropar API:et hälsokontroll
                    if (response.IsSuccessStatusCode)//om API:et svarar med 200
                    {
                        apiStatuses[api.Key] = "Running";//lägger till Running-status
                    }
                    else
                    {
                        apiStatuses[api.Key] = "Issue Detected";//API:et svarar, men inte korrekt
                    }
                }
                catch
                {
                    apiStatuses[api.Key] = "Down";//API:et kunde inte nås
                }
            }
            return Ok(apiStatuses);//returnerar status för API:erna 
        }



        [HttpGet("stats")]//end-point för att hämta statistik från API:er
        public async Task<IActionResult> GetApiStatistics()
        {
            var apis = new Dictionary<string, string>//dictionary med API:ernas statistik-url
            {
                { "LoginService", "https://localhost:7130/api/stats/usage" },
                { "UserService", "https://localhost:7200/api/stats/usage" },
                { "BookingService", "https://localhost:7300/api/stats/usage" }
            };

            var apiStats = new Dictionary<string, List<ApiStat>>();//dict för att lagra statistik

            foreach (var api in apis)//för varje API i dict
            {
                try
                {
                    var response = await _httpClient.GetAsync(api.Value);//anropar API:ets statistik-url
                    var content = await response.Content.ReadAsStringAsync();//läser svaret som en string

                    Console.WriteLine($"API Response från {api.Key}: {content}");//debugging, skriver ut svaret i konsollen

                    var stats = JsonConvert.DeserializeObject<List<ApiStat>>(content);//konvererar JSON till en lista med ApiStat-objekt
                    apiStats[api.Key] = stats ?? new List<ApiStat>();//om listan är null, ersätter med en tom lista
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fel vid hämtning av statistik från {api.Key}: {ex.Message}");//loggar ett felmeddelande om API:et inte kan nås.
                    apiStats[api.Key] = new List<ApiStat> { new ApiStat { Endpoint = "Error", RequestCount = 0 } };//lägger till en felindikator.
                }
            }

            return Ok(apiStats);//returnera API-statistiken
        }

    }

}
