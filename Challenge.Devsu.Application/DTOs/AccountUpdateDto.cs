using Challenge.Devsu.Application.Validators;
using Challenge.Devsu.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Devsu.Application.DTOs
{
    public class AccountUpdateDto
    {
        [Required(ErrorMessage = "El identificador de la cuenta es obligatorio para la actualizaci�n.")]
        [NotEmptyGuid]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "El n�mero de cuenta es obligatorio.")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "El n�mero de cuenta debe tener entre 4 y 30 caracteres.")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de cuenta es obligatorio.")]
        [EnumDataType(typeof(AccountType), ErrorMessage = "Tipo de cuenta inv�lido.")]
        public AccountType AccountType { get; set; }

        [Required]
        public bool Active { get; set; } = true;
    }
}
