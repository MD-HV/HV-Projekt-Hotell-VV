using Microsoft.EntityFrameworkCore;

namespace CleaningAPI
{
    public class CleaningDbContext : DbContext
    {
        public DbSet<CleaningTask> CleaningTasks { get; set; }
        public DbSet<ApiUsageLog> ApiUsageLogs { get; set; }
        public DbSet<User> Users { get; set; }

        public CleaningDbContext(DbContextOptions<CleaningDbContext> opt) 
            : base(opt) 
        { 

        }
    }
}
