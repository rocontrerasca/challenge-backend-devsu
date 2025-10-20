using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Application.UseCases;
using Challenge.Devsu.Shared.Helpers;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _clientUseCase.GetAllAsync();
                return ApiResponseHelper.CreateNewListSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando clientes");
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ClientDto dto)
        {
            try
            {
                var response = await _clientUseCase.CreateAsync(dto);
                await _logUseCase.Create(response.ClientId, "Cliente creado exitosamente");
                return ApiResponseHelper.CreateNewListSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando cliente");
                await _logUseCase.Create(null, "Error al crear cliente: " + ex.Message);
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var response = await _clientUseCase.GetByIdAsync(id);
                return ApiResponseHelper.ReadSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando cliente");
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClientUpdateDto dto)
        {
            try
            {
                var response = await _clientUseCase.UpdateAsync(dto);
                await _logUseCase.Create(response.ClientId, "Cliente actualizado exitosamente");
                return ApiResponseHelper.UpdateSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el cliente con ID {Id}", dto.ClientId);
                await _logUseCase.Create(dto.ClientId, "Error al actualizar cliente: " + ex.Message);
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var response = await _clientUseCase.DeleteByIdAsync(id);
                await _logUseCase.Create(response.ClientId, "Cliente eliminado exitosamente");
                return ApiResponseHelper.ReadSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error borrando el cliente.");
                await _logUseCase.Create(id, "Error al eliminar cliente: " + ex.Message);
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
