using Challenge.Devsu.Core.Enums;

namespace Challenge.Devsu.Application.DTOs
{
    public class AccountResponseDto
    {
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public AccountType AccountType { get; set; }
        public decimal InitialBalance { get; set; }
        public bool Active { get; set; } = true;

        public Guid ClientRefId { get; set; }
        public ClientDto Client { get; set; } = default!;

        public ICollection<MoveDto> Movements { get; set; } = new List<MoveDto>();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
