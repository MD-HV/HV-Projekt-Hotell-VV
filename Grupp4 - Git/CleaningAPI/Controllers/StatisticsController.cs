using Microsoft.AspNetCore.Mvc;
using CleaningAPI;

namespace CleaningAPI.Controllers
{
    [ApiController]

    [Route("api/stats")]//endpoit för att hämta statistik 

    public class StatisticsController : ControllerBase

    {

        private readonly CleaningDbContext _context; //dependency injection som gör att vi kan läsa interagera med DB. 



        public StatisticsController(CleaningDbContext context)

        {//konstruktor som använder dependency injection databaskontexten till den lokala variabeln 

            _context = context;

        }



        [HttpGet("usage")]

        public IActionResult GetUsageStats()

        {

            //hämtar och grupperar API-anropslista baserat på endpoints och räknar hur många gånger de anropats. 

            var stats = _context.ApiUsageLogs //hämtar alla API-loggar från DB 

                .GroupBy(log => log.Endpoint)//gruppeerar loggarna baserat på vilken endpoint som anropats 

                .Select(group => new //skapar ett objekt för att lagra statistik 

                {

                    Endpoint = group.Key, //nyckeln, dvs. endpointen 

                    RequestCount = group.Count() //räknar antalet anrop till endpointen 

                })

                .OrderByDescending(x => x.RequestCount)//sorterar efter mest anropade 

                .ToList();//omvandlar resultatet till en lista som kan returnas i JSON. 



            return Ok(stats);//returnerar 200 Ok tillsammans med listan. 

        }

    }
}
