using Challenge.Devsu.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Devsu.Application.DTOs
{
    public class ClientResponseDto
    {
        public Guid ClientId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Age { get; set; }
        public string IdentificationNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Active { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<AccountResponseDto> Accounts { get; set; } = [];
    }
}
