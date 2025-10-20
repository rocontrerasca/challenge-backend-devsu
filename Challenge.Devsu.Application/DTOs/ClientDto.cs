using Challenge.Devsu.Core.Entities;
using Challenge.Devsu.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Devsu.Application.DTOs
{
    public class ClientDto
    {

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "La contraseña debe tener entre 4 y 20 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public bool Active { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(120, ErrorMessage = "El nombre completo no puede exceder los 120 caracteres.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El género es obligatorio.")]
        [StringLength(2, ErrorMessage = "El género debe tener máximo 2 caracteres (por ejemplo: M o F).")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [Range(0, 120, ErrorMessage = "La edad debe estar entre 0 y 120 años.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "El número de identificación es obligatorio.")]
        [StringLength(20, ErrorMessage = "El número de identificación no puede exceder los 20 caracteres.")]
        public string IdentificationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [StringLength(10, ErrorMessage = "El número de teléfono no puede exceder los 10 caracteres.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
