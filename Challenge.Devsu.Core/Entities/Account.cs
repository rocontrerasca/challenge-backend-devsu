using Challenge.Devsu.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenge.Devsu.Core.Entities;

public partial class Account
{
    [Key]
    [Column("account_id")]
    public Guid AccountId { get; set; }
    [Column("account_number")]
    public string AccountNumber { get; set; } = string.Empty;
    [Column("account_type")]
    public AccountType AccountType { get; set; }
    [Column("initial_balance")]
    public decimal InitialBalance { get; set; }
    [Column("active")]
    public bool Active { get; set; } = true;

    [ForeignKey(nameof(Client))]
    [Column("client_ref_id")]
    public Guid ClientRefId { get; set; }
    public Client Client { get; set; } = default!;

    public ICollection<Move> Movements { get; set; } = new List<Move>();
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
