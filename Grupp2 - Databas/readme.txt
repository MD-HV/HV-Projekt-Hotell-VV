Hur fungerar API:et?

Detta kan du göra med API:et:
-Registrera nya användare
-Logga in och skapa en session
-Kontrollera om en användare är inloggad
-Logga ut
-Radera en användare (endast admin)


API-endpoints

POST
/api/auth/register
Skapar en ny användare

POST
/api/auth/login
Loggar in en användare

GET
/api/auth/protected
Skyddad endpoint som kräver inloggning

POST
/api/auth/logout
Loggar ut en användare

DELETE
/api/auth/delete/{userId}
Raderar en användare (endast admin)


Så här använder du API:et

1. Registrera en användare
Skicka en POST-förfrågan till:
/api/auth/register
Body (JSON):
{
  "username": "exempelnamn",
  "passwordHash": "exempellösenord",
  "roleID": 3
}
OBS:
* Om ingen roll anges blir användaren automatiskt gäst (roleID: 3).
* Administratörskonton kan inte skapas via API:et.
* För att skapa en anställd (roleID: 2) krävs en adminToken i Headers.

MVC-kodexempel i en Controller:
public async Task<IActionResult> Register()
{
    var client = new HttpClient();
    var requestBody = new
    {
        username = "exempelnamn",
        passwordHash = "exempellösenord",
        roleID = 3
    };

    var response = await client.PostAsJsonAsync("https://localhost:7086/api/auth/register", requestBody);

    if (response.IsSuccessStatusCode)
    {
        return Content("Registrering lyckades!");
    }
    return Content("Registrering misslyckades.");
}



2. Logga in en användare
Skicka en POST-förfrågan till:
/api/auth/login
Body (JSON):
{
  "username": "exempelnamn",
  "passwordHash": "exempellösenord"
}
Svar:
{
  "message": "Inloggning lyckades!",
  "sessionId": "uniktSessionsId"
}
Efter inloggning sparas en cookie (SessionID) som används vid fortsatta API-anrop.

MVC-kodexempel i en Controller:

public async Task<IActionResult> Login()
{
    var client = new HttpClient();
    var requestBody = new
    {
        username = "exempelnamn",
        passwordHash = "exempellösenord"
    };

    var response = await client.PostAsJsonAsync("https://localhost:7086/api/auth/login", requestBody);
    var responseContent = await response.Content.ReadAsStringAsync();

    return Content(responseContent);
}


3. Anropa en skyddad endpoint
Skicka en GET-förfrågan till:
/api/auth/protected
Svar om användaren är inloggad:
{
  "message": "Du är inloggad!",
  "userId": 1,
  "userRole": 2
}
Om sessionen gått ut:
{
  "message": "Sessionen har gått ut, vänligen logga in igen."
}

MVC-kodexempel i en Controller:

public async Task<IActionResult> CheckLoginStatus()
{
    var client = new HttpClient();
    var response = await client.GetAsync("https://localhost:7086/api/auth/protected");

    var responseContent = await response.Content.ReadAsStringAsync();
    return Content(responseContent);
}

 4. Logga ut en användare
Skicka en POST-förfrågan till:
 /api/auth/logout
Svar:
{
  "message": "Du har loggats ut och sessionen har rensats."
}

MVC-kodexempel i en Controller:

public async Task<IActionResult> Logout()
{
    var client = new HttpClient();
    var response = await client.PostAsync("https://localhost:7086/api/auth/logout", null);

    var responseContent = await response.Content.ReadAsStringAsync();
    return Content(responseContent);
}


 5. Ta bort en användare (endast admin)
Skicka en DELETE-förfrågan till:
/api/auth/delete/{userId}
Lägg till en adminToken i Headers:
adminToken: "dittAdminToken"
Svar:
{
  "message": "Användaren med ID 5 har tagits bort."
}

MVC-kodexempel i en Controller:

public async Task<IActionResult> DeleteUser(int userId)
{
    var client = new HttpClient();
    client.DefaultRequestHeaders.Add("adminToken", "dittAdminToken");

    var response = await client.DeleteAsync($"https://localhost:7086/api/auth/delete/{userId}");
    var responseContent = await response.Content.ReadAsStringAsync();

    return Content(responseContent);
}

Hur skickar man en adminToken i ett API-anrop?
När en administratör loggar in via /api/auth/login får de en session. För att använda adminToken behöver man skicka hashen av adminens lösenord som en header i API-anropen.
När du skickar en begäran där adminToken krävs, lägger du till den i Headers så här:
adminToken: "dinAdminLösenordHash"

Exempel:
Om en administratör vill skapa en anställd (roleID: 2) via /api/auth/register, skickar de en POST-förfrågan med sin adminToken i Headers:

Begäran:
POST /api/auth/register
Content-Type: application/json
adminToken: "adminensLösenordHash"
Body (JSON):
{
  "username": "nyAnställd",
  "passwordHash": "nyttLösenord",
  "roleID": 2
}

Exempel: Skapa en anställd (roleID: 2) i en MVC-controller

public async Task<IActionResult> CreateEmployee()
{
    var client = new HttpClient();
    client.DefaultRequestHeaders.Add("adminToken", "adminensLösenordHash");

    var requestBody = new
    {
        username = "nyAnställd",
        passwordHash = "nyttLösenord",
        roleID = 2
    };

    var response = await client.PostAsJsonAsync("https://localhost:7086/api/auth/register", requestBody);
    var responseContent = await response.Content.ReadAsStringAsync();

    return Content(responseContent);
}

Viktiga saker att tänka på
-API:et använder cookies för sessioner – se till att din klient hanterar cookies korrekt.
-En användares session varar i 30 minuter innan den löper ut.
-Endast administratörer kan ta bort användare och skapa konton med roleID 2 (anställd).
