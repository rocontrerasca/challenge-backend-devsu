using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Challenge.Devsu.Core.Entities
{
    public class Log
    {
        [Key]
        [Column("log_id")]
        public long LogId { get; set; }
        [Column("resource_id")]
        public Guid? ResourceId { get; set; }
        [Column("message")]
        public string Message { get; set; } = string.Empty;
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
