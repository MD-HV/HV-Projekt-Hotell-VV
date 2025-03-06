
namespace CleaningService.Models
{
    public class CleaningAPI
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int StaffId { get; set; }
        public bool IsCleaning { get; set; }
        public string? ReportedDamages { get; set; }
    }
}