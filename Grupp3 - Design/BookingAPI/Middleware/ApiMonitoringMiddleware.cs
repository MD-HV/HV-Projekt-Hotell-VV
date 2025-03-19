using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using BookingAPI.Models;//använd era bibliotek här 

using System;



public class ApiLoggingMiddleware

{

    private readonly RequestDelegate _next;



    public ApiLoggingMiddleware(RequestDelegate next)

    {

        _next = next;

    }



    public async Task Invoke(HttpContext context, BookingDbContext dbContext)

    {

        if (context.Request.Path.StartsWithSegments("/api"))//om anropet är ett apianrop 

        {

            var logEntry = new ApiUsageLog

            {

                Endpoint = context.Request.Path.ToString(),//sparar anropets mål som endpoint 

                Timestamp = DateTime.UtcNow//sätter tiden till vad klockan är nu 

            };



            dbContext.ApiUsageLogs.Add(logEntry);//lägger till logEntry som nytt objekt i DB. 

            await dbContext.SaveChangesAsync();//sparar 

        }



        await _next(context); //fortsätter till nästa steg i pipeline:n 

    }

}