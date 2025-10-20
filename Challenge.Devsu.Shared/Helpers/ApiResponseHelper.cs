using Challenge.Devsu.Core.ExceptionDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Challenge.Devsu.Shared.Helpers
{
    public static class ApiResponseHelper
    {
        private const string UuidKey = "uuid";
        private const string TimestampKey = "timestamp";
        private const string SystemIdKey = "systemId";

        private static object GetMeta(HttpContext httpContext) => new
        {
            UuId = httpContext.Items[UuidKey]?.ToString(),
            Timestamp = httpContext.Items[TimestampKey]?.ToString(),
            SystemId = httpContext.Items[SystemIdKey]?.ToString()
        };
        public static IActionResult ReadSuccessResponse(HttpContext httpContext, object data)
        {
            var response = new
            {
                Meta = GetMeta(httpContext),
                StatusCodigo = 200,
                StatusDesc = "OK",
                AdicionalInfo = new[]
            {
                new { Codigo = "200-299", Detalle = "Operación exitosa." }
            },
                data
            };

            return new OkObjectResult(response);
        }
        public static IActionResult UpdateSuccessResponse<T>(HttpContext httpContext, T data)
        {
            var response = new
            {
                Meta = GetMeta(httpContext),
                StatusCodigo = 200,
                StatusDesc = "OK",
                AdicionalInfo = new[]
            {
                new { Codigo = "100-999", Detalle = "Actualización exitosa." }
            },
                data
            };

            return new OkObjectResult(response);
        }

        public static IActionResult CreateNewRecordSuccessResponse<T>(HttpContext httpContext, T data)
        {
            var response = new
            {
                Meta = GetMeta(httpContext),
                StatusCodigo = 201,
                StatusDesc = "OK",
                AdicionalInfo = new[]
            {
                new { Codigo = "100-999", Detalle = "Operación exitosa." }
            },
                data
            };

            return new OkObjectResult(response);
        }
        public static IActionResult CreateNewListSuccessResponse(HttpContext httpContext, object data)
        {
            var response = new
            {
                Meta = GetMeta(httpContext),
                StatusCodigo = 201,
                StatusDesc = "OK",
                AdicionalInfo = new[]
            {
                new { Codigo = "100-999", Detalle = "Operación exitosa." }
            },
                data
            };

            return new OkObjectResult(response);
        }
        public static IActionResult CreateNotFoundResponse(HttpContext httpContext, string message, string errorCode = "404-001", object? data = null)
        {
            var response = new
            {
                Meta = GetMeta(httpContext),
                StatusCodigo = 404,
                StatusDesc = "Not Found",
                AdicionalInfo = new[]
            {
                new { Codigo = errorCode, Detalle = message }
            },
                Data = data
            };

            return new NotFoundObjectResult(response);
        }

        public static IActionResult CreateInternalErrorResponse(HttpContext httpContext, string message, string errorCode = "500-001")
        {
            var response = new
            {
                Meta = GetMeta(httpContext),
                StatusCodigo = 500,
                StatusDesc = "Error interno del servidor",
                AdicionalInfo = new[]
            {
                new { Codigo = errorCode, Detalle = message }
            },
                Data = (object)null!
            };

            return new ObjectResult(response) { StatusCode = 500 };
        }

        public static IActionResult CreateDomainErrorResponse(HttpContext httpContext, string message, string errorCode, int code)
        {
            var response = new
            {
                Meta = GetMeta(httpContext),
                StatusCodigo = code,
                StatusDesc = errorCode,
                AdicionalInfo = new[]
            {
                new { Codigo = errorCode, Detalle = message }
            },
                Data = (object)null!
            };

            return new ObjectResult(response) { StatusCode = code };
        }
    }
}
