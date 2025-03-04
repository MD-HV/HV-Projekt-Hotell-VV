Hur fungerar API:et?

Detta kan du g�ra med API:et:
-Registrera nya anv�ndare
-Logga in och skapa en session
-Kontrollera om en anv�ndare �r inloggad
-Logga ut
-Radera en anv�ndare (endast admin)


API-endpoints

POST
/api/auth/register
Skapar en ny anv�ndare

POST
/api/auth/login
Loggar in en anv�ndare

GET
/api/auth/protected
Skyddad endpoint som kr�ver inloggning

POST
/api/auth/logout
Loggar ut en anv�ndare

DELETE
/api/auth/delete/{userId}
Raderar en anv�ndare (endast admin)


S� h�r anv�nder du API:et

1. Registrera en anv�ndare
Skicka en POST-f�rfr�gan till:
/api/auth/register
Body (JSON):
{
  "username": "exempelnamn",
  "passwordHash": "exempell�senord",
  "roleID": 3
}
OBS:
* Om ingen roll anges blir anv�ndaren automatiskt g�st (roleID: 3).
* Administrat�rskonton kan inte skapas via API:et.
* F�r att skapa en anst�lld (roleID: 2) kr�vs en adminToken i Headers.

MVC-kodexempel i en Controller:
public async Task<IActionResult> Register()
{
    var client = new HttpClient();
    var requestBody = new
    {
        username = "exempelnamn",
        passwordHash = "exempell�senord",
        roleID = 3
    };

    var response = await client.PostAsJsonAsync("https://localhost:7086/api/auth/register", requestBody);

    if (response.IsSuccessStatusCode)
    {
        return Content("Registrering lyckades!");
    }
    return Content("Registrering misslyckades.");
}



2. Logga in en anv�ndare
Skicka en POST-f�rfr�gan till:
/api/auth/login
Body (JSON):
{
  "username": "exempelnamn",
  "passwordHash": "exempell�senord"
}
Svar:
{
  "message": "Inloggning lyckades!",
  "sessionId": "uniktSessionsId"
}
Efter inloggning sparas en cookie (SessionID) som anv�nds vid fortsatta API-anrop.

MVC-kodexempel i en Controller:

public async Task<IActionResult> Login()
{
    var client = new HttpClient();
    var requestBody = new
    {
        username = "exempelnamn",
        passwordHash = "exempell�senord"
    };

    var response = await client.PostAsJsonAsync("https://localhost:7086/api/auth/login", requestBody);
    var responseContent = await response.Content.ReadAsStringAsync();

    return Content(responseContent);
}


3. Anropa en skyddad endpoint
Skicka en GET-f�rfr�gan till:
/api/auth/protected
Svar om anv�ndaren �r inloggad:
{
  "message": "Du �r inloggad!",
  "userId": 1,
  "userRole": 2
}
Om sessionen g�tt ut:
{
  "message": "Sessionen har g�tt ut, v�nligen logga in igen."
}

MVC-kodexempel i en Controller:

public async Task<IActionResult> CheckLoginStatus()
{
    var client = new HttpClient();
    var response = await client.GetAsync("https://localhost:7086/api/auth/protected");

    var responseContent = await response.Content.ReadAsStringAsync();
    return Content(responseContent);
}

 4. Logga ut en anv�ndare
Skicka en POST-f�rfr�gan till:
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


 5. Ta bort en anv�ndare (endast admin)
Skicka en DELETE-f�rfr�gan till:
/api/auth/delete/{userId}
L�gg till en adminToken i Headers:
adminToken: "dittAdminToken"
Svar:
{
  "message": "Anv�ndaren med ID 5 har tagits bort."
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
N�r en administrat�r loggar in via /api/auth/login f�r de en session. F�r att anv�nda adminToken beh�ver man skicka hashen av adminens l�senord som en header i API-anropen.
N�r du skickar en beg�ran d�r adminToken kr�vs, l�gger du till den i Headers s� h�r:
adminToken: "dinAdminL�senordHash"

Exempel:
Om en administrat�r vill skapa en anst�lld (roleID: 2) via /api/auth/register, skickar de en POST-f�rfr�gan med sin adminToken i Headers:

Beg�ran:
POST /api/auth/register
Content-Type: application/json
adminToken: "adminensL�senordHash"
Body (JSON):
{
  "username": "nyAnst�lld",
  "passwordHash": "nyttL�senord",
  "roleID": 2
}

Exempel: Skapa en anst�lld (roleID: 2) i en MVC-controller

public async Task<IActionResult> CreateEmployee()
{
    var client = new HttpClient();
    client.DefaultRequestHeaders.Add("adminToken", "adminensL�senordHash");

    var requestBody = new
    {
        username = "nyAnst�lld",
        passwordHash = "nyttL�senord",
        roleID = 2
    };

    var response = await client.PostAsJsonAsync("https://localhost:7086/api/auth/register", requestBody);
    var responseContent = await response.Content.ReadAsStringAsync();

    return Content(responseContent);
}

Viktiga saker att t�nka p�
-API:et anv�nder cookies f�r sessioner � se till att din klient hanterar cookies korrekt.
-En anv�ndares session varar i 30 minuter innan den l�per ut.
-Endast administrat�rer kan ta bort anv�ndare och skapa konton med roleID 2 (anst�lld).
