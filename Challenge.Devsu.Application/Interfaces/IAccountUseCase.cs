
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Core.Entities;

namespace Challenge.Devsu.Application.Interfaces
{
    public interface IAccountUseCase
    {
        Task<AccountResponseDto> CreateAsync(AccountDto requestDto);
        Task<AccountResponseDto> DeleteByIdAsync(Guid id);
        Task<IEnumerable<AccountResponseDto>> GetAllAsync();
        Task<IEnumerable<AccountResponseDto>> GetByClientId(Guid clientId);
        Task<AccountResponseDto> GetByIdAsync(Guid id);
        Task<AccountUpdateDto> UpdateAsync(AccountUpdateDto requestDto);
    }
}
