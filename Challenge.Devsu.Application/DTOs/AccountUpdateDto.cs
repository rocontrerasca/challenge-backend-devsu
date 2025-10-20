using Challenge.Devsu.Application.Validators;
using Challenge.Devsu.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Devsu.Application.DTOs
{
    public class AccountUpdateDto
    {
        [Required(ErrorMessage = "El identificador de la cuenta es obligatorio para la actualización.")]
        [NotEmptyGuid]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "El tipo de cuenta es obligatorio.")]
        [EnumDataType(typeof(AccountType), ErrorMessage = "Tipo de cuenta inválido.")]
        public AccountType AccountType { get; set; }

        [Required]
        public bool Active { get; set; } = true;
    }
}
