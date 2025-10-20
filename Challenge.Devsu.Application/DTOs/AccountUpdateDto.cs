using Challenge.Devsu.Core.Enums;

namespace Challenge.Devsu.Application.DTOs
{
    public class AccountUpdateDto
    {
        public Guid AccountId { get; set; }
        public AccountType AccountType { get; set; }
        public bool Active { get; set; } = true;
    }
}
