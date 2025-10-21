using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Core.Enums;
using Challenge.Devsu.Core.ExceptionDomain;
using Challenge.Devsu.Core.Response;
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

        /// <summary>
        /// COnsulta de movimientos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MoveResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _moveUseCase.GetAllAsync();
                return ApiResponse<IEnumerable<MoveResponseDto>>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando movimientos");
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Creación de movimiento
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MoveResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> Create([FromBody] MoveDto dto)
        {
            try
            {
                var response = await _moveUseCase.CreateAsync(dto);
                await _logUseCase.Create(response.MoveId, $"Movimiento creado exitosamente: {(response.MoveType == MoveType.Debito ? "Retiro" : "Depósito")} de {response.Amount}");
                return ApiResponse<MoveResponseDto>.CreateResponse(HttpContext, 201, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando movimiento");
                await _logUseCase.Create(null, "Error al crear movimiento: " + ex.Message);
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Consulta de movimiento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<MoveResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var response = await _moveUseCase.GetByIdAsync(id);
                return ApiResponse<MoveResponseDto>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando movimiento");
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }        

        /// <summary>
        /// Consulta de movimientos por cuenta
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("account/{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MoveResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> GetByAccountId(Guid accountId)
        {
            try
            {
                var response = await _moveUseCase.GetByAccountIdAsync(accountId);
                return ApiResponse<IEnumerable<MoveResponseDto>>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando movimientos cuenta");
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Generación reporte
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("report")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MoveReportResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> GetMoveReport(MoveReportDto requestDto)
        {
            try
            {
                var response = await _moveUseCase.GetMoveReportAsync(requestDto);
                return ApiResponse<IEnumerable<MoveReportResponseDto>>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando reporte movimientos cuenta");
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }

        /// <summary>
        /// Generación reporte base64
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("report/pdf")]
        [ProducesResponseType(typeof(ApiResponse<MoveReportPdfResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 500)]
        public async Task<IActionResult> GetMoveReportPdf(MoveReportDto requestDto)
        {
            try
            {
                var response = await _moveUseCase.GetMoveReportPdfAsync(requestDto);
                return ApiResponse<MoveReportPdfResponseDto>.CreateResponse(HttpContext, 200, "OK", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando reporte movimientos cuenta");
                if (ex is DomainException de)
                {
                    return ApiResponse<string>.CreateResponse<string>(HttpContext, de.Code, de.CodeDescription, de.Message);
                }
                return ApiResponse<string>.CreateResponse<string>(HttpContext, 500, null!, ex.Message);
            }
        }
    }
}
