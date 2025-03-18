using System.ComponentModel.DataAnnotations;
namespace BookingAPI.Models
{
    public class DinnerTable
    {
        [Key] 
        public int TableID { get; set; }
        public int Seats { get; set; }
        public string Location { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}
