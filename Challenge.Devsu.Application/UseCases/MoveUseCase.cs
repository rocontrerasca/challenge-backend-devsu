using AutoMapper;
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Core.Entities;
using Challenge.Devsu.Core.ExceptionDomain;
using Challenge.Devsu.Core.Interfaces;

namespace Challenge.Devsu.Application.UseCases
{
    public class MoveUseCase : IMoveUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMoveRepository _moveRepository;
        private readonly IMapper _mapper;

        public MoveUseCase(IAccountRepository accountRepository, IMapper mapper, IMoveRepository moveRepository)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _moveRepository = moveRepository;
        }

        public async Task<IEnumerable<MoveResponseDto>> GetAllAsync()
        {
            var clientList = await _moveRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MoveResponseDto>>(clientList);
        }

        public async Task<MoveResponseDto> GetByIdAsync(Guid id)
        {
            var existingEntity = (await _moveRepository.FindAsync(q => q.MoveId == id)).FirstOrDefault() ?? throw new NotFoundException("movimiento", id);
            return _mapper.Map<MoveResponseDto>(existingEntity);
        }

        public async Task<MoveResponseDto> CreateAsync(MoveDto requestDto)
        {
            if (requestDto == null)
            {
                throw new BusinessRuleException("El objeto no puede ser nulo.");
            }
            var existingClientEntity = (await _accountRepository.FindAsync(q => q.AccountId == requestDto.AccountRefId)).FirstOrDefault() ?? throw new NotFoundException("cuenta", requestDto.AccountRefId);
            var entity = _mapper.Map<Move>(requestDto);
            var insertedEntity = await _moveRepository.AddAsync(entity);
            return _mapper.Map<MoveResponseDto>(insertedEntity);
        }
        
        public async Task<IEnumerable<MoveResponseDto>> GetByAccountId(Guid accountId)
        {
            _ = (await _accountRepository.FindAsync(q => q.AccountId == accountId)).FirstOrDefault() ?? throw new NotFoundException("cuenta", accountId);
            var clientList = await _accountRepository.FindAsync(q => q.AccountId == accountId);
            return _mapper.Map<IEnumerable<MoveResponseDto>>(clientList);
        }
    }
}
