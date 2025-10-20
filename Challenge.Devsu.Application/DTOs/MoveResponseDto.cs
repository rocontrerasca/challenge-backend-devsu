using Challenge.Devsu.Core.Enums;

namespace Challenge.Devsu.Application.DTOs;

public partial class MoveResponseDto
{
    public Guid MoveId { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public MoveType MoveType { get; set; }
    public decimal Amount { get; set; }
    public decimal InitialBalance { get; set; }

    public Guid AccountRefId { get; set; }
    public bool Success { get; set; }
    public AccountDto Account { get; set; } = default!;
}
