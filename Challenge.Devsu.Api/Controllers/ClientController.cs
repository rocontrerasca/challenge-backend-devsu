using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Core.ExceptionDomain;
using Challenge.Devsu.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Devsu.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientUseCase _clientUseCase;
        private readonly ILogger<ClientController> _logger;
        private readonly ILogUseCase _logUseCase;
        public ClientController(IClientUseCase clientUseCase, ILogger<ClientController> logger, ILogUseCase logUseCase)
        {
            _clientUseCase = clientUseCase;
            _logger = logger;
            _logUseCase = logUseCase;
        }

        /// <summary>
        /// Consulta de listado de clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ClientResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _clientUseCase.GetAllAsync();
                return ApiResponse<IEnumerable<ClientResponseDto>>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando clientes");
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Creación de cliente
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ClientResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> Create([FromBody]ClientDto dto)
        {
            try
            {
                var response = await _clientUseCase.CreateAsync(dto);
                await _logUseCase.Create(response.ClientId, "Cliente creado exitosamente");
                return ApiResponse<ClientResponseDto>.CreateResponse(HttpContext, 201, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando cliente");
                await _logUseCase.Create(null, "Error al crear cliente: " + ex.Message);
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Consulta de cliente por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<ClientResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var response = await _clientUseCase.GetByIdAsync(id);
                return ApiResponse<ClientResponseDto>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando cliente");
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Actualización de cliente
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<ClientUpdateDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> Update([FromBody] ClientUpdateDto dto)
        {
            try
            {
                var response = await _clientUseCase.UpdateAsync(dto);
                await _logUseCase.Create(response.ClientId, "Cliente actualizado exitosamente");
                return ApiResponse<ClientUpdateDto>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el cliente con ID {Id}", dto.ClientId);
                await _logUseCase.Create(dto.ClientId, "Error al actualizar cliente: " + ex.Message);
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Eliminación de cliente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<ClientResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var response = await _clientUseCase.DeleteByIdAsync(id);
                await _logUseCase.Create(response.ClientId, "Cliente eliminado exitosamente");
                return ApiResponse<ClientResponseDto>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error borrando el cliente.");
                await _logUseCase.Create(id, "Error al eliminar cliente: " + ex.Message);
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }
    }
}
