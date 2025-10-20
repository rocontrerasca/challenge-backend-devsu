using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Application.UseCases;
using Challenge.Devsu.Core.ExceptionDomain;
using Challenge.Devsu.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Devsu.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountUseCase _accountUseCase;
        private readonly ILogger<AccountController> _logger;
        private readonly ILogUseCase _logUseCase;

        public AccountController(IAccountUseCase accountUseCase, ILogger<AccountController> logger, ILogUseCase logUseCase)
        {
            _accountUseCase = accountUseCase;
            _logger = logger;
            _logUseCase = logUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _accountUseCase.GetAllAsync();                
                return ApiResponseHelper.CreateNewListSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando cuentas");
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountDto dto)
        {
            try
            {
                var response = await _accountUseCase.CreateAsync(dto);
                await _logUseCase.Create(response.AccountId, "Cuenta creada exitosamente");
                return ApiResponseHelper.CreateNewListSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando cuenta");
                await _logUseCase.Create(null, "Error al crear cuenta: " + ex.Message);
                if (ex is DomainException de)
                {
                    return ApiResponseHelper.CreateDomainErrorResponse(
                        HttpContext, de.Message, de.CodeDescription, de.Code);
                }
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var response = await _accountUseCase.GetByIdAsync(id);
                return ApiResponseHelper.ReadSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando cuenta");
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AccountUpdateDto dto)
        {
            try
            {
                var response = await _accountUseCase.UpdateAsync(dto);
                await _logUseCase.Create(response.AccountId, "Cuenta actualizada exitosamente");
                return ApiResponseHelper.UpdateSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el cliente con ID {Id}", dto.AccountId);
                await _logUseCase.Create(dto.AccountId, "Error al actualizar cuenta: " + ex.Message);
                if (ex is DomainException de)
                {
                    return ApiResponseHelper.CreateDomainErrorResponse(
                        HttpContext, de.Message, de.CodeDescription, de.Code);
                }
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var response = await _accountUseCase.DeleteByIdAsync(id);
                await _logUseCase.Create(response.AccountId, "Cuenta eliminada exitosamente");
                return ApiResponseHelper.ReadSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error borrando el cuenta.");
                await _logUseCase.Create(id, "Error al eliminar cuenta: " +  ex.Message);
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("client/{id:guid}")]
        public async Task<IActionResult> GetByClientId(Guid clientId)
        {
            try
            {
                var response = await _accountUseCase.GetByClientId(clientId);
                return ApiResponseHelper.ReadSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando cuentas por cliente");
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
