// Challenge.Devsu.Domain/Exceptions/DomainException.cs
namespace Challenge.Devsu.Core.ExceptionDomain;

public abstract class DomainException : Exception
{
    protected DomainException(string message, string code) : base(message) => Code = code;
    public string Code { get; }
}

public sealed class NotFoundException : DomainException
{
    public NotFoundException(string resource, object? key = null)
        : base(key is null ? $"{resource} no encontrado." : $"{resource} {key} no encontrado.",
               "not_found")
    { }
}

public sealed class ConflictException : DomainException
{
    public ConflictException(string message) : base(message, "conflict") { }
}

public sealed class ValidationException : DomainException
{
    public ValidationException(string message, IDictionary<string, string[]> errors)
        : base(message, "validation_failed") => Errors = errors;
    public IDictionary<string, string[]> Errors { get; }
}

public sealed class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message) : base(message, "business_rule_violation") { }
}
