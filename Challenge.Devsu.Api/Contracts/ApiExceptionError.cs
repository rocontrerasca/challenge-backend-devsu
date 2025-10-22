namespace Challenge.Devsu.Api.Contracts
{
    public sealed record ApiExceptionError(
    int status,
    string code,
    string title,
    string detail,
    string traceId,
    IDictionary<string, string[]>? errors = null);
}
