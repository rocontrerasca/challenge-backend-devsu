
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Core.Entities;

namespace Challenge.Devsu.Application.Interfaces
{
    public interface IMoveUseCase
    {
        Task<MoveResponseDto> CreateAsync(MoveDto requestDto);
        Task<IEnumerable<MoveResponseDto>> GetAllAsync();
        Task<IEnumerable<MoveResponseDto>> GetByAccountIdAsync(Guid accountId);
        Task<MoveResponseDto> GetByIdAsync(Guid id);
        Task<IEnumerable<MoveReportResponseDto>> GetMoveReportAsync(MoveReportDto requestDto);
    }
}
