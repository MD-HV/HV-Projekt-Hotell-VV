using CleaningAPI;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Configuration;
using CleaningAPI.Models;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

// Add session services
builder.Services.AddDistributedMemoryCache(); // This enables the in-memory session store
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout to 30 minutes (or your preferred value)
    options.Cookie.IsEssential = true; // Make sure the cookie is essential (needed for GDPR compliance)
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseMiddleware<ApiLoggingMiddleware>();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.Run();
