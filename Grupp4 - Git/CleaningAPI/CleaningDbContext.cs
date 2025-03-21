using Microsoft.EntityFrameworkCore;

namespace CleaningAPI
{
    public class CleaningDbContext : DbContext
    {
        

        public CleaningDbContext(DbContextOptions<CleaningDbContext> opt) 
            : base(opt) 
        { 

        }
        public DbSet<CleaningTask> CleaningTasks { get; set; }
        public DbSet<ApiUsageLog> ApiUsageLogs { get; set; }
    }
}
