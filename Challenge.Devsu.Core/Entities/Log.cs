using System.Text.Json;

namespace Challenge.Devsu.Core.Entities
{
    public class Log
    {
        public Guid LogId { get; set; } 
        public Guid? ResourceId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? Date { get; set; } = DateTime.UtcNow;
    }
}
