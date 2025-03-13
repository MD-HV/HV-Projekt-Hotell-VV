using Microsoft.EntityFrameworkCore;

namespace CleaningAPI
{
    [PrimaryKey(nameof(Id))]
    public class Task
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int StaffId { get; set; }
        public bool IsCleaning { get; set; }
        public string? ReportedDamages { get; set; }
    }
}
