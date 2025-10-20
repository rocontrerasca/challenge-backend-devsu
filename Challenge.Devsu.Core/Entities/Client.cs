using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenge.Devsu.Core.Entities;

public partial class Client: Person
{
    [Key]
    [Column("client_id")]
    public Guid ClientId { get; set; }
    [Column("password")]
    public string Password { get; set; } = string.Empty;
    [Column("active")]
    public bool Active { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public ICollection<Account> Accounts { get; set; } = [];
}
