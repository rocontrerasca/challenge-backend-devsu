// Challenge.Devsu.Api/Middlewares/ExceptionMiddleware.cs
using Challenge.Devsu.Api.Contracts;
using Challenge.Devsu.Core.ExceptionDomain;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ValidationException = Challenge.Devsu.Core.ExceptionDomain.ValidationException;

namespace Challenge.Devsu.Api.Middlewares;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var traceId = context.TraceIdentifier;
            _logger.LogError(ex, "Unhandled exception. TraceId: {TraceId}", traceId);
            await WriteProblemAsync(context, ex, traceId);
        }
    }

    private async Task WriteProblemAsync(HttpContext ctx, Exception ex, string traceId)
    {
        if (ctx.Response.HasStarted)
        {
            _logger.LogWarning("La respuesta ya se había iniciado. No se puede escribir el error. TraceId: {TraceId}", traceId);
            return;
        }

        var (status, code, title, detail, errors) = MapException(ex, _env.IsDevelopment());

        var payload = new ApiExceptionError(
            status: (int)status,
            code: code,
            title: title,
            detail: detail,
            traceId: traceId,
            errors: errors
        );

        ctx.Response.ContentType = "application/json; charset=utf-8";
        ctx.Response.StatusCode = (int)status;

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        await ctx.Response.WriteAsync(json);
    }


    private static (HttpStatusCode status, string code, string title, string detail, IDictionary<string, string[]>? errors)
        MapException(Exception ex, bool isDev)
    {
        // 1) Excepciones de dominio (tu jerarquía)
        if (ex is ValidationException vex)
            return ((HttpStatusCode)422, vex.Code.ToString(), "Error de validación", vex.Message, vex.Errors);

        if (ex is NotFoundException nfx)
            return (HttpStatusCode.NotFound, nfx.Code.ToString(), "Recurso no encontrado", nfx.Message, null);

        if (ex is ConflictException cfx)
            return (HttpStatusCode.Conflict, cfx.Code.ToString(), "Conflicto", cfx.Message, null);

        if (ex is BusinessRuleException brx)
            return (HttpStatusCode.BadRequest, brx.Code.ToString(), "Regla de negocio", brx.Message, null);

        // 2) Validaciones de DataAnnotations (si aparecieran)
        if (ex is System.ComponentModel.DataAnnotations.ValidationException svx)
            return ((HttpStatusCode)422, "validation_failed", "Error de validación", svx.Message, null);

        // 3) EF Core / PostgreSQL
        if (ex is DbUpdateConcurrencyException)
            return (HttpStatusCode.Conflict, "concurrency_conflict", "Conflicto de concurrencia",
                    "El recurso fue modificado por otro proceso.", null);

        if (ex is DbUpdateException dbex && dbex.InnerException is PostgresException pg)
        {
            return pg.SqlState switch
            {
                PostgresErrorCodes.UniqueViolation => (HttpStatusCode.Conflict, "unique_violation", "Conflicto de unicidad",
                                    "Ya existe un registro con el mismo valor único.", null),

                PostgresErrorCodes.ForeignKeyViolation => (HttpStatusCode.Conflict, "foreign_key_violation", "Violación de clave foránea",
                                       "Violación de integridad referencial.", null),

                _ => (HttpStatusCode.BadRequest, "db_error", "Error de base de datos",
                      GetPgDetail(pg, "Error en la base de datos."), null)
            };
        }

        // 4) Petición/entrada mal formada
        if (ex is BadHttpRequestException bx)
            return (HttpStatusCode.BadRequest, "bad_request", "Solicitud inválida", bx.Message, null);

        if (ex is JsonException jx)
            return (HttpStatusCode.BadRequest, "invalid_json", "JSON inválido",
                    isDev ? jx.ToString() : "El cuerpo JSON no es válido.", null);

        // 5) Control de flujo/timeout/cancelación
        if (ex is OperationCanceledException or TaskCanceledException)
            // 499 Client Closed Request (no estándar HTTP, pero común) o usa 408 si prefieres estándar
            return ((HttpStatusCode)499, "client_closed_request", "Solicitud cancelada por el cliente",
                    "La operación fue cancelada.", null);

        if (ex is TimeoutException)
            return (HttpStatusCode.GatewayTimeout, "timeout", "Tiempo de espera agotado",
                    "La operación excedió el tiempo de espera.", null);

        // 6) Otros comunes
        if (ex is KeyNotFoundException kx)
            return (HttpStatusCode.NotFound, "not_found", "No encontrado", kx.Message, null);

        if (ex is UnauthorizedAccessException ux)
            return (HttpStatusCode.Forbidden, "forbidden", "Acceso denegado", ux.Message, null);

        if (ex is ArgumentException aex)
            return (HttpStatusCode.BadRequest, "invalid_argument", "Argumento inválido", aex.Message, null);

        // 7) Desconocido
        var safeDetail = isDev ? ex.ToString() : "Se produjo un error inesperado. Inténtalo nuevamente.";
        return (HttpStatusCode.InternalServerError, "internal_error", "Error interno", safeDetail, null);
    }

    private static string GetPgDetail(PostgresException pg, string fallback)
        => string.IsNullOrWhiteSpace(pg.Detail) ? fallback : pg.Detail;

}
