
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Devsu.Core.Response
{
    public class ApiResponse<T>
    {
        public MetaData Meta { get; set; }
        public int StatusCodigo { get; set; }
        public string StatusDesc { get; set; }
        public List<DetalleInfo> AdicionalInfo { get; set; }
        public T Data { get; set; }

        public ApiResponse()
        {
            Meta = new MetaData();
            AdicionalInfo = new List<DetalleInfo>();
            StatusDesc = string.Empty;
            Data = default!;
        }
        public ApiResponse(int statusCodigo, string statusDesc, T data, List<DetalleInfo>? adicionalInfo = null)
        {
            Meta = new MetaData();
            StatusCodigo = statusCodigo;
            StatusDesc = statusDesc;
            AdicionalInfo = adicionalInfo ?? new List<DetalleInfo>();
            Data = data;
        }

        public static IActionResult CreateResponse<TData>(
            HttpContext httpContext,
            int statusCodigo,
            string statusDesc,
            TData data,
            List<DetalleInfo>? adicionalInfo = null)
        {
            var response = new
            {
                Meta = new MetaData
                {
                    UuId = Guid.NewGuid().ToString(),
                    Timestamp = DateTime.UtcNow.ToLongDateString(),
                    SystemId = "dev"
                },
                StatusCodigo = statusCodigo,
                StatusDesc = statusDesc,
                AdicionalInfo = adicionalInfo ?? new List<DetalleInfo>(),
                Data = data
            };

            return statusCodigo switch
            {
                200 => new OkObjectResult(response),
                201 => new ObjectResult(response) { StatusCode = 201 },
                400 => new BadRequestObjectResult(response),
                404 => new NotFoundObjectResult(response),
                500 => new ObjectResult(response) { StatusCode = 500 },
                _ => new ObjectResult(response) { StatusCode = statusCodigo }
            };
        }
    }
}
