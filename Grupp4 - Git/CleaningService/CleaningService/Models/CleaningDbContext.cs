﻿using Microsoft.EntityFrameworkCore;

namespace CleaningService.Models
{
    public class CleaningDbContext : DbContext
    {
        public DbSet<CleaningTask> Tasks { get; set; }
        public DbSet<CleaningAPI> TaskAPI { get; set; }

        public CleaningDbContext(DbContextOptions<CleaningDbContext> option) : base(option) { }
    }
}
