using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;

    public AccountController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string Username, string Password)
    {
        var loginRequest = new
        {
            username = Username,
            passwordHash = Password
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://localhost:7130/api/auth/login", jsonContent);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var sessionData = JsonConvert.DeserializeObject<dynamic>(responseBody);
            var sessionId = sessionData?.sessionId;

            if (sessionId != null)
            {
                Response.Cookies.Append("SessionID", sessionId.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                return RedirectToAction("Health", "Monitoring");
            }
        }

        ViewBag.ErrorMessage = "Fel användarnamn eller lösenord.";
        return View();
    }

    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("SessionID");
        return RedirectToAction("Login");
    }
}
