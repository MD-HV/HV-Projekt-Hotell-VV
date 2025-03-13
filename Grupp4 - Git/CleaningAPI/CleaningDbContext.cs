using Microsoft.EntityFrameworkCore;

namespace CleaningAPI
{
    public class CleaningDbContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public CleaningDbContext(DbContextOptions<CleaningDbContext> opt) 
            : base(opt) 
        { 

        }
    }
}
