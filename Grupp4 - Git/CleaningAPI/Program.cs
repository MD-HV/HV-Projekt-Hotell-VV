using CleaningAPI;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

string connString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<CleaningDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found!"));
}); //B�r �ndras om vi ska k�ra vanliga SQLservrar! UseSqlite("Data Source=CleaningAPI.db") <--SQL Local = SQL server localt, Default = Jan-olofs log in på sin server

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
