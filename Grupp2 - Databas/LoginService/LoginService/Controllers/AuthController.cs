/*Controller som hanterar autentisering och användarhantering.*/
using LoginService.Models;
using LoginService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[ApiController]
[Route("api/auth")]//definierad URL för controllern
public class AuthController : ControllerBase
{

    private readonly LoginServiceDbContext _context;

    public AuthController(LoginServiceDbContext context)
    {
        _context = context;
    }//https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-9.0#create-the-database-context
    /* Skapar en instans av databaskontexten. I länkan ovan förklaras hur konstruktorn tar emot databaskontexten via Dependency Injection.*/


    
    [HttpGet("protected")]//skyddad endpoint som bara funkar om användaren har en aktiv session
    public async Task<IActionResult> ProtectedEndpointAsync()
    {

        var logEntry = new ApiUsageLog //skapar ett nytt objekt av ApiUsageLog
        {
            Endpoint = "/auth/protected",//sparar endpoint
            Timestamp = DateTime.UtcNow//sparar tiden
        };
        _context.ApiUsageLogs.Add(logEntry);//lägger till ApiUsageLog i DB
        await _context.SaveChangesAsync();//sparar DB

        var sessionId = HttpContext.Session.GetString("SessionID"); //hämtar sessionsID och lagrar i var 'sessionId'

        if (string.IsNullOrEmpty(sessionId))//om sessionId är tom så körs detta kodstycke
        {
            return Unauthorized(new { message = "Sessionen har gått ut, vänligen logga in igen." });//returnerar att sessionen har gått ut.
        }

        var userId = HttpContext.Session.GetInt32("UserID");//hämtar userID och sparar i var 'userId'
        var userRole = HttpContext.Session.GetInt32("UserRole"); //hämtar rollen och sparar i 'userRole'

        return Ok(new { message = "Du är inloggad!", userId, userRole });//returnerar 200 OK, och ett bekräftande meddelande
    }



    [HttpGet("login/health")]//url för monitoringAPI:et
    public async Task<IActionResult> HealthCheckAsync()
    {
        var logEntry = new ApiUsageLog //skapar ett nytt objekt av ApiUsageLog
        {
            Endpoint = "/auth/login/health",//sparar endpoint
            Timestamp = DateTime.UtcNow//sparar tiden
        };
        _context.ApiUsageLogs.Add(logEntry);//lägger till ApiUsageLog i DB
        await _context.SaveChangesAsync();//sparar DB

        return Ok(new { status = "Login API Running" });//returnerar 200 Ok, funktion som gjordes för övervakningsAPI:et.

    }


    private bool IsUserLoggedIn()//funktion som används i controllern
    {
        var sessionId = HttpContext.Session.GetString("SessionID");//hämtar sessionsID 
        return !string.IsNullOrEmpty(sessionId);//returnerar True om sessionsID finns, annars False.
    }



    [HttpPost("register")]//register endpoint, hade mycket problem vid införandet av GUI så en del GPT-hjälp nedan.
    public async Task<IActionResult> Register([FromBody] User user, [FromHeader] string? adminToken)//tar emot User-modell via JSON.
    {

        var logEntry = new ApiUsageLog //skapar ett nytt objekt av ApiUsageLog
        {
            Endpoint = "/auth/register",//sparar endpoint
            Timestamp = DateTime.UtcNow//sparar tiden
        };
        _context.ApiUsageLogs.Add(logEntry);//lägger till ApiUsageLog i DB
        await _context.SaveChangesAsync();//sparar DB

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username); //kontrollerar om det redan finns en användare med angivet användarnamn
        if (existingUser != null) return BadRequest("Användarnamnet är upptaget.");//returnerar att användarnamnet är taget

        if (user.RoleID == 0) user.RoleID = 3;//om ingen roll anges så är standard 3, vilket är "gäst".

        if (user.RoleID == 1) //kontrollerar om kontot som försöker skapas är admin
        {
            return Unauthorized("Det går inte att skapa administratörskonton via registrerings-API:et.");//returnerar att admin inte går att skapas via API:et
        }

        if (user.RoleID == 2)//kontrollerar om kontot som skapas ska vara anställd
        {
            if (string.IsNullOrEmpty(adminToken))//om förfrågan saknar adminToken
            {
                return Unauthorized("Endast administratörer kan skapa anställda.");//returnerar detta
            }

            var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.PasswordHash == adminToken && u.RoleID == 1); //kontrollerar den medskickade adminToken
            if (adminUser == null)//om ogiltig adminToken
            {
                return Unauthorized("Ogiltig admin-token.");//returneras detta
            }
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);//lösenordet hashas för lagring på databas

        _context.Users.Add(user);//lägger till användaren i databasen
        await _context.SaveChangesAsync();//sparar daatabasen

        return Ok("Användare skapad.");//returnerar 200 Ok och bekräftelse
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login) //tar emot en LoginRequest via JSON
    {
        if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.PasswordHash))//kontrollerar att indata inte är null eller tomma strängar
        {
            return BadRequest("Användarnamn och lösenord krävs.");//400 Bad Request
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);//söker efter angiven användare i databasen
        if (user == null) return Unauthorized("Fel användarnamn eller lösenord.");//om ingen användare hittas 401 Unauthorized

        if (!BCrypt.Net.BCrypt.Verify(login.PasswordHash, user.PasswordHash))//jämför inskickat lösenord med det lagrade hashade lösenordet
        {
            return Unauthorized("Fel användarnamn eller lösenord.");//returnerar 401 Unauthorized om fel.
        }

        var logEntry = new ApiUsageLog //skapar ett nytt objekt av ApiUsageLog
        {
            Endpoint = "/auth/login",//sparar endpoint
            Timestamp = DateTime.UtcNow//sparar tiden
        };

        _context.ApiUsageLogs.Add(logEntry);//lägger till ApiUsageLog i DB
        await _context.SaveChangesAsync();//sparar DB

        Console.WriteLine($"Inloggning lyckades för: {user.Username}");//om inloggning lyckas i konsollen (felsökningsfunktion)

        var sessionId = Guid.NewGuid().ToString(); //Genererar en unik SessionsID (nedan är GPT-tips.)
        HttpContext.Session.SetString("SessionID", sessionId);//lagrar sessionsId
        HttpContext.Session.SetInt32("UserID", user.UserID);//lagrar UserID
        HttpContext.Session.SetInt32("UserRole", user.RoleID);//lagrar RoleID

        Response.Cookies.Append("SessionID", sessionId, new CookieOptions//sätter en cookie med sessionsid så att klienten kan identifiera förfrågningar
        {
            HttpOnly = true,  //förhindrar att JS kan läsa cookien
            Secure = false,   //false för utveckling, ska vara "true" vid HTTPS-trafik.
            SameSite = SameSiteMode.None, //Gpt-tips, krävs för att cookies ska skickas i cross-origin requests (CORS)
            Expires = DateTime.UtcNow.AddMinutes(30)//sessionen upphör efter 30 minuters inaktivitet
        });

        return Ok(new { message = "Inloggning lyckades!", sessionId });//bekräftelse, 200 Ok
    }




    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId, [FromHeader] string? adminToken)//tar emot userID som ska raderas och eventuellt en adminToken
    {
        var logEntry = new ApiUsageLog //skapar ett nytt objekt av ApiUsageLog
        {
            Endpoint = "/auth/delete",//sparar endpoint
            Timestamp = DateTime.UtcNow//sparar tiden
        };
        _context.ApiUsageLogs.Add(logEntry);//lägger till ApiUsageLog i DB
        await _context.SaveChangesAsync();//sparar DB


        if (string.IsNullOrEmpty(adminToken))//kontrollerar att admintoken är med
        {
            return Unauthorized("Endast administratörer kan ta bort användare.");//401 Unautorized
        }

        var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.PasswordHash == adminToken && u.RoleID == 1);//kontrollerar att en admin finns med den medföljande adminToken.
        if (adminUser == null)//om det inte finns en admin med den adminToken
        {
            return Unauthorized("Ogiltig admin-token.");//401
        }

        var userToDelete = await _context.Users.FindAsync(userId);//hämtar användaren som ska raderas
        if (userToDelete == null)//om användaren inte finns
        {
            return NotFound("Användaren kunde inte hittas.");//404
        }

        if (adminUser.UserID == userToDelete.UserID)//om inloggad admin och den som ska tas bort matchar
        {
            return BadRequest("Du kan inte ta bort ditt eget administratörskonto.");//returnerar att admin inte kan ta bort sig själv.
        }

        _context.Users.Remove(userToDelete);//tar bort användaren ur databasen
        await _context.SaveChangesAsync();//sparar

        return Ok($"Användaren med ID {userId} har tagits bort.");//bekräftelse
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {

        var logEntry = new ApiUsageLog //skapar ett nytt objekt av ApiUsageLog
        {
            Endpoint = "/auth/logout",//sparar endpoint
            Timestamp = DateTime.UtcNow//sparar tiden
        };
        _context.ApiUsageLogs.Add(logEntry);//lägger till ApiUsageLog i DB
        await _context.SaveChangesAsync();//sparar DB

        Console.WriteLine("Loggar ut användare, rensar session och cookie.");//Skriver en bekräftelse i Console (felsökningskomponent)

        HttpContext.Session.Clear(); //rensar sessionsdata
        Response.Cookies.Delete("SessionID"); //tar bort sessionscookien ifrån webbläsaren

        return Ok(new { message = "Du har loggats ut och sessionen har rensats." });//bekräftelse, 200 Ok
    }




}
