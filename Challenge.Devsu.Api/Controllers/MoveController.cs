using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Application.UseCases;
using Challenge.Devsu.Core.Enums;
using Challenge.Devsu.Core.ExceptionDomain;
using Challenge.Devsu.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Challenge.Devsu.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoveController : ControllerBase
    {
        private readonly IMoveUseCase _moveUseCase;
        private readonly ILogger<MoveController> _logger;
        private readonly ILogUseCase _logUseCase;
        public MoveController(IMoveUseCase moveUseCase, ILogger<MoveController> logger, ILogUseCase logUseCase)
        {
            _moveUseCase = moveUseCase;
            _logger = logger;
            _logUseCase = logUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _moveUseCase.GetAllAsync();
                return ApiResponseHelper.CreateNewListSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando movimientos");
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MoveDto dto)
        {
            try
            {
                var response = await _moveUseCase.CreateAsync(dto);
                await _logUseCase.Create(response.MoveId, $"Movimiento creado exitosamente: {(response.MoveType == MoveType.Debito ? "Retiro" : "Depósito")} de {response.Amount}");
                return ApiResponseHelper.CreateNewListSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando movimiento");
                await _logUseCase.Create(null, "Error al crear movimiento: " + ex.Message);
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
                var response = await _moveUseCase.GetByIdAsync(id);
                return ApiResponseHelper.ReadSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando movimiento");
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }        

        [HttpGet("account/{id:guid}")]
        public async Task<IActionResult> GetByAccountId(Guid accountId)
        {
            try
            {
                var response = await _moveUseCase.GetByAccountIdAsync(accountId);
                return ApiResponseHelper.ReadSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando movimientos cuenta");
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("report")]
        public async Task<IActionResult> GetMoveReport(MoveReportDto requestDto)
        {
            try
            {
                var response = await _moveUseCase.GetMoveReportAsync(requestDto);
                return ApiResponseHelper.ReadSuccessResponse(HttpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando reporte movimientos cuenta");
                return ApiResponseHelper.CreateInternalErrorResponse(HttpContext, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
