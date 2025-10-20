// Challenge.Devsu.Domain/Exceptions/DomainException.cs
namespace Challenge.Devsu.Core.ExceptionDomain;

public abstract class DomainException : Exception
{
    protected DomainException(string message, string codeDescription, int code) : base(message)
    {
        CodeDescription = codeDescription;
        Code = code;
    }
    public string CodeDescription { get; }
    public int Code { get; }
}

public sealed class NotFoundException : DomainException
{
    public NotFoundException(string resource, object? key = null)
        : base(key is null ? $"{resource} no encontrado." : $"{resource} {key} no encontrado.",
               "not_found", 404)
    { }
}

public sealed class ConflictException : DomainException
{
    public ConflictException(string message) : base(message, "conflict", 409) { }
}

public sealed class ValidationException : DomainException
{
    public ValidationException(string message, IDictionary<string, string[]> errors)
        : base(message, "validation_failed", 400) => Errors = errors;
    public IDictionary<string, string[]> Errors { get; }
}

public sealed class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message) : base(message, "business_rule_violation", 500) { }
}
