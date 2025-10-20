using Challenge.Devsu.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Devsu.Core.Entities;

public partial class Account
{
    [Key]
    public Guid AccountId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public decimal InitialBalance { get; set; }
    public bool Active { get; set; } = true;

    public Guid ClientRefId { get; set; }
    public Client Client { get; set; } = default!;

    public ICollection<Move> Movements { get; set; } = new List<Move>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
