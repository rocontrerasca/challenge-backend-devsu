using Challenge.Devsu.Application.Validators;
using Challenge.Devsu.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Devsu.Application.DTOs
{

    public class AccountDto
    {
        [Required(ErrorMessage = "El número de cuenta es obligatorio.")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "El número de cuenta debe tener entre 4 y 30 caracteres.")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de cuenta es obligatorio.")]
        [EnumDataType(typeof(AccountType), ErrorMessage = "Tipo de cuenta inválido.")]
        public AccountType AccountType { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El balance debe ser mayor o igual a 0.")]
        public decimal InitialBalance { get; set; }

        [Required]
        public bool Active { get; set; } = true;

        [Required(ErrorMessage = "El cliente asociado es obligatorio.")]
        [NotEmptyGuid]
        public Guid ClientRefId { get; set; }
    }
}
