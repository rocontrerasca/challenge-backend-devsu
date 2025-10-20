// Challenge.Devsu.Api/Middlewares/ExceptionMiddleware.cs
using Challenge.Devsu.Api.Contracts;
using Challenge.Devsu.Core.ExceptionDomain;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
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
        var (status, code, title, detail, errors) = MapException(ex, _env.IsDevelopment());

        var payload = new ApiExceptionError(
            status: (int)status,
            code: code,
            title: title,
            detail: detail,
            traceId: traceId,
            errors: errors
        );

        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)status;

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        });

        await ctx.Response.WriteAsync(json);
    }

    private static (HttpStatusCode status, string code, string title, string detail, IDictionary<string, string[]>? errors)
        MapException(Exception ex, bool isDev)
    {
        // 1) Excepciones de dominio
        if (ex is ValidationException vex)
            return (HttpStatusCode.UnprocessableEntity, vex.Code, "Error de validación", vex.Message, vex.Errors);

        if (ex is NotFoundException nfx)
            return (HttpStatusCode.NotFound, nfx.Code, "Recurso no encontrado", nfx.Message, null);

        if (ex is ConflictException cfx)
            return (HttpStatusCode.Conflict, cfx.Code, "Conflicto", cfx.Message, null);

        if (ex is BusinessRuleException brx)
            return (HttpStatusCode.BadRequest, brx.Code, "Regla de negocio", brx.Message, null);

        // 2) EF Core / Postgres
        if (ex is DbUpdateConcurrencyException)
            return (HttpStatusCode.Conflict, "concurrency_conflict", "Conflicto de concurrencia", "El recurso fue modificado por otro proceso.", null);

        if (ex is DbUpdateException dbex && dbex.InnerException is PostgresException pg)
        {
            // Códigos más comunes de Postgres (SQLSTATE)
            // 23505 = unique_violation, 23503 = foreign_key_violation
            return pg.SqlState switch
            {
                "23505" => (HttpStatusCode.Conflict, "unique_violation", "Conflicto de unicidad", GetPgDetail(pg, "Ya existe un registro con el mismo valor único."), null),
                "23503" => (HttpStatusCode.Conflict, "foreign_key_violation", "Violación de clave foránea", GetPgDetail(pg, "Violación de integridad referencial."), null),
                _ => (HttpStatusCode.BadRequest, "db_error", "Error de base de datos", GetPgDetail(pg, "Error en la base de datos."), null)
            };
        }

        // 3) Argumentos inválidos
        if (ex is ArgumentException aex)
            return (HttpStatusCode.BadRequest, "invalid_argument", "Argumento inválido", aex.Message, null);

        // 4) Desconocido
        var safeDetail = isDev ? ex.ToString() : "Se produjo un error inesperado. Inténtalo nuevamente.";
        return (HttpStatusCode.InternalServerError, "internal_error", "Error interno", safeDetail, null);
    }

    private static string GetPgDetail(PostgresException pg, string fallback)
    {
        // Puedes enriquecer con pg.ConstraintName, pg.TableName, etc.
        return string.IsNullOrWhiteSpace(pg.Detail) ? fallback : pg.Detail;
    }
}
