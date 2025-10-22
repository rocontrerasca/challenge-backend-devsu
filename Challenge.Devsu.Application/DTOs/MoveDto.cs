using Challenge.Devsu.Application.Validators;
using Challenge.Devsu.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Challenge.Devsu.Application.DTOs;

public class MoveDto
{
    [Required(ErrorMessage = "El tipo de movimiento es obligatorio.")]
    [EnumDataType(typeof(MoveType), ErrorMessage = "Tipo de movimiento inválido.")]
    public MoveType MoveType { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que 0.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "La cuenta asociada es obligatoria.")]
    [NotEmptyGuid]
    public Guid AccountRefId { get; set; }
}
