using AutoMapper;
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Core.Entities;
using Challenge.Devsu.Core.ExceptionDomain;
using Challenge.Devsu.Core.Interfaces;

namespace Challenge.Devsu.Application.UseCases
{
    public class ClientUseCase : IClientUseCase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientUseCase(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientResponseDto>> GetAllAsync()
        {
            var clientList = await _clientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClientResponseDto>>(clientList);
        }

        public async Task<ClientResponseDto> GetByIdAsync(Guid id)
        {
            var existingEntity = (await _clientRepository.FindAsync(q => q.ClientId == id)).FirstOrDefault() ?? throw new NotFoundException("cliente", id);
            return _mapper.Map<ClientResponseDto>(existingEntity);
        }

        public async Task<ClientResponseDto> CreateAsync(ClientDto requestDto)
        {
            if (requestDto == null)
            {
                throw new BusinessRuleException("El objeto no puede ser nulo.");
            }
            if ((await _clientRepository.FindAsync(q => q.IdentificationNumber == requestDto.IdentificationNumber)).FirstOrDefault() != null)
            {
                throw new ConflictException($"Ya existe un cliente con la identificación {requestDto.IdentificationNumber}");
            }
            var entity = _mapper.Map<Client>(requestDto);
            var insertedEntity = await _clientRepository.AddAsync(entity);
            return _mapper.Map<ClientResponseDto>(insertedEntity);
        }
        public async Task<ClientUpdateDto> UpdateAsync(ClientUpdateDto requestDto)
        {
            if (requestDto == null)
                throw new BusinessRuleException("El objeto no puede ser nulo.");

            var existingEntity = (await _clientRepository.FindAsync(q => q.ClientId == requestDto.ClientId)).FirstOrDefault() ?? throw new NotFoundException("cliente", requestDto.ClientId);
            _mapper.Map(requestDto, existingEntity);
            await _clientRepository.UpdateAsync(existingEntity);
            return _mapper.Map<ClientUpdateDto>(existingEntity);
        }

        public async Task<ClientResponseDto> DeleteByIdAsync(Guid id)
        {
            var existingEntity = (await _clientRepository.FindAsync(q => q.ClientId == id)).FirstOrDefault() ?? throw new NotFoundException("cliente", id);
            if (existingEntity.Accounts.Count > 0) throw new BusinessRuleException("Cliente tiene cuentas relacionadas");
            if (existingEntity.Accounts.Where(a => a.Movements.Count > 0).Any()) throw new BusinessRuleException("Cliente tiene cuentas relacionadas con movimientos");
            await _clientRepository.RemoveAsync(existingEntity);
            return _mapper.Map<ClientResponseDto>(existingEntity);
        }
    }
}
