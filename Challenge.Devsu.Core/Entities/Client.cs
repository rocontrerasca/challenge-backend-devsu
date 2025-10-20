using System.ComponentModel.DataAnnotations;

namespace Challenge.Devsu.Core.Entities;

public partial class Client: Person
{
    [Key]
    public Guid ClientId { get; set; }
    public string Password { get; set; } = string.Empty;
    public bool Active { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<Account> Accounts { get; set; } = [];
}
