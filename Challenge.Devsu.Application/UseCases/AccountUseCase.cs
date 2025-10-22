using AutoMapper;
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Core.Entities;
using Challenge.Devsu.Core.ExceptionDomain;
using Challenge.Devsu.Core.Interfaces;

namespace Challenge.Devsu.Application.UseCases
{
    public class AccountUseCase : IAccountUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public AccountUseCase(IAccountRepository accountRepository, IMapper mapper, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<AccountResponseDto>> GetAllAsync()
        {
            var clientList = await _accountRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AccountResponseDto>>(clientList);
        }

        public async Task<AccountResponseDto> GetByIdAsync(Guid id)
        {
            var existingEntity = (await _accountRepository.FindAsync(q => q.AccountId == id)).FirstOrDefault() ?? throw new NotFoundException("cuenta", id);
            return _mapper.Map<AccountResponseDto>(existingEntity);
        }

        public async Task<AccountResponseDto> CreateAsync(AccountDto requestDto)
        {
            if (requestDto == null)
            {
                throw new BusinessRuleException("El objeto no puede ser nulo.");
            }
            var existingClientEntity = (await _clientRepository.FindAsync(q => q.ClientId == requestDto.ClientRefId)).FirstOrDefault() ?? throw new NotFoundException("cliente", requestDto.ClientRefId);
            var entity = _mapper.Map<Account>(requestDto);
            var insertedEntity = await _accountRepository.AddAsync(entity);
            return _mapper.Map<AccountResponseDto>(insertedEntity);
        }
        public async Task<AccountUpdateDto> UpdateAsync(AccountUpdateDto requestDto)
        {
            if (requestDto == null)
                throw new BusinessRuleException("El objeto no puede ser nulo.");

            var existingEntity = (await _accountRepository.FindAsync(q => q.AccountId == requestDto.AccountId)).FirstOrDefault() ?? throw new NotFoundException("cuenta", requestDto.AccountId);
            existingEntity = _mapper.Map(requestDto, existingEntity);
            await _accountRepository.UpdateAsync(existingEntity);
            return _mapper.Map<AccountUpdateDto>(existingEntity);
        }

        public async Task<AccountResponseDto> DeleteByIdAsync(Guid id)
        {
            var existingEntity = (await _accountRepository.FindAsync(q => q.AccountId == id)).FirstOrDefault() ?? throw new NotFoundException("cuenta", id);
            if (existingEntity.Movements.Count > 0) throw new BusinessRuleException("Cuenta tiene relacionada movimientos");
            await _accountRepository.RemoveAsync(existingEntity);
            return _mapper.Map<AccountResponseDto>(existingEntity);
        }

        public async Task<IEnumerable<AccountResponseDto>> GetByClientId(Guid clientId)
        {
            _ = (await _clientRepository.FindAsync(q => q.ClientId == clientId)).FirstOrDefault() ?? throw new NotFoundException("cliente", clientId);
            var clientList = await _accountRepository.FindAsync(q => q.ClientRefId == clientId);
            return _mapper.Map<IEnumerable<AccountResponseDto>>(clientList);
        }
    }
}
