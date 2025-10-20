using System.ComponentModel.DataAnnotations;

namespace Challenge.Devsu.Application.Validators
{
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        public NotEmptyGuidAttribute() => ErrorMessage = "El identificador no puede ser vac�o.";
        public override bool IsValid(object? value) => value is Guid g && g != Guid.Empty;
    }
}
