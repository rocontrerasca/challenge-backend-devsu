using Challenge.Devsu.Application.Validators;
using Challenge.Devsu.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Challenge.Devsu.Application.DTOs;

public class MoveReportDto
{
    [Required]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "El id del cliente es obligatorio.")]
    [NotEmptyGuid]
    public Guid ClientId { get; set; }
}
