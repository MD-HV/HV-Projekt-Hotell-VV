namespace MenuAPI.Models
{
    public class ApiUsageLog
    {
        public int Id { get; set; }
        public string? Endpoint { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
