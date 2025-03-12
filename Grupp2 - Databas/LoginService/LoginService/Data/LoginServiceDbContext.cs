using Microsoft.EntityFrameworkCore;
using LoginService.Models;

namespace LoginService.Data
{
    public class LoginServiceDbContext : DbContext
    //DbContext skapad i enlighet med Microsoft standarden: https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-9.0#create-the-database-context
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ApiUsageLog> ApiUsageLogs { get; set; }

        public LoginServiceDbContext(DbContextOptions<LoginServiceDbContext> options) : base(options) { }
    }
}
