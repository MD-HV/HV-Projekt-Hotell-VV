using CleaningAPI;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<CleaningDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("LocalConnection"))); //B�r �ndras om vi ska k�ra vanliga SQLservrar! UseSqlite("Data Source=CleaningAPI.db") <--SQL Local = SQL server localt, Default = Jan-olofs log in på sin server

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
