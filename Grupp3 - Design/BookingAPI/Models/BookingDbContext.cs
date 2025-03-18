using Microsoft.EntityFrameworkCore;
namespace BookingAPI.Models

{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<DinnerTable> DinnerTables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<DinnerTable>().ToTable("DinnerTable");

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.DinnerTable)
                .WithMany(d => d.Bookings)
                .HasForeignKey(b => b.TableID_FK)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
