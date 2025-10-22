using Challenge.Devsu.Core.Enums;

namespace Challenge.Devsu.Application.DTOs;

public class MoveReportResponseDto
{
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string Client {  get; set; } = string.Empty;
    public string Account {  get; set; } = string.Empty;
    public string AccountType {  get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal InitialBalance { get; set; }
    public bool Success { get; set; }
    public decimal FinalBalance { get; set; }
}
