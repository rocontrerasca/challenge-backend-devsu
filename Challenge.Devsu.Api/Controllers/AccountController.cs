using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Core.ExceptionDomain;
using Challenge.Devsu.Core.Response;
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

        /// <summary>
        /// Consulta listado cuentas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AccountResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _accountUseCase.GetAllAsync();
                return ApiResponse<IEnumerable<AccountResponseDto>>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando cuentas");
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Creacion cuenta
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<AccountResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> Create([FromBody] AccountDto dto)
        {
            try
            {
                var response = await _accountUseCase.CreateAsync(dto);
                await _logUseCase.Create(response.AccountId, "Cuenta creada exitosamente");
                return ApiResponse<AccountResponseDto>.CreateResponse(HttpContext, 201, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando cuenta");
                await _logUseCase.Create(null, "Error al crear cuenta: " + ex.Message);
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Consulta de cuenta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<AccountResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var response = await _accountUseCase.GetByIdAsync(id);
                return ApiResponse<AccountResponseDto>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando cuenta");
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Actualizacion cuenta
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<AccountUpdateDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> Update([FromBody] AccountUpdateDto dto)
        {
            try
            {
                var response = await _accountUseCase.UpdateAsync(dto);
                await _logUseCase.Create(response.AccountId, "Cuenta actualizada exitosamente");
                return ApiResponse<AccountUpdateDto>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el cliente con ID {Id}", dto.AccountId);
                await _logUseCase.Create(dto.AccountId, "Error al actualizar cuenta: " + ex.Message);
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Borrado cuenta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<AccountResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var response = await _accountUseCase.DeleteByIdAsync(id);
                await _logUseCase.Create(response.AccountId, "Cuenta eliminada exitosamente");
                return ApiResponse<AccountResponseDto>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error borrando el cuenta.");
                await _logUseCase.Create(id, "Error al eliminar cuenta: " +  ex.Message);
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Consulta cuenta por cliente
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("client/{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<AccountResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> GetByClientId(Guid clientId)
        {
            try
            {
                var response = await _accountUseCase.GetByClientId(clientId);
                return ApiResponse<AccountResponseDto>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando cuentas por cliente");
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }
    }
}
