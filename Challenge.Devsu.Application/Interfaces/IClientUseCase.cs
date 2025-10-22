
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Core.Entities;

namespace Challenge.Devsu.Application.Interfaces
{
    public interface IClientUseCase
    {
        Task<ClientResponseDto> CreateAsync(ClientDto requestDto);
        Task<ClientResponseDto> DeleteByIdAsync(Guid id);
        Task<IEnumerable<ClientResponseDto>> GetAllAsync();
        Task<ClientResponseDto> GetByIdAsync(Guid id);
        Task<ClientUpdateDto> UpdateAsync(ClientUpdateDto requestDto);
    }
}
