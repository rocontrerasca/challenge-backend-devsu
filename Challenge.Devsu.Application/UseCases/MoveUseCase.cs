using AutoMapper;
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Application.Interfaces;
using Challenge.Devsu.Core.Entities;
using Challenge.Devsu.Core.Enums;
using Challenge.Devsu.Core.ExceptionDomain;
using Challenge.Devsu.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Challenge.Devsu.Application.UseCases
{
    public class MoveUseCase : IMoveUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMoveRepository _moveRepository;
        private readonly IMapper _mapper;
        private readonly decimal _dailyLimit;

        public MoveUseCase(IAccountRepository accountRepository, IMapper mapper, IMoveRepository moveRepository, IConfiguration cfg)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _moveRepository = moveRepository;
            _ = decimal.TryParse(cfg["LIMITE_DIARIO_RETIRO"], out _dailyLimit);
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
            if (requestDto is null)
                throw new BusinessRuleException("El objeto no puede ser nulo.");

            Account? existingAccountEntity = null;
            decimal? currentBalance = null;

            try
            {
                // 1) Cargar cuenta o lanzar NotFound (esto NO será atrapado por el catch filtrado)
                existingAccountEntity = (await _accountRepository
                    .FindAsync(q => q.AccountId == requestDto.AccountRefId))
                    .FirstOrDefault()
                    ?? throw new NotFoundException("cuenta", requestDto.AccountRefId);

                if (!existingAccountEntity.Active)
                    throw new BusinessRuleException("Cuenta inactiva.");

                // 2) Calcular saldo actual y validar reglas
                currentBalance = existingAccountEntity.Movements
                    .OrderByDescending(m => m.TransactionDate)
                    .Select(m => m.Balance)
                    .FirstOrDefault(existingAccountEntity.InitialBalance);

                var amount = Math.Abs(requestDto.Amount);
                var isDebit = requestDto.MoveType == MoveType.Debito;

                if (isDebit)
                {
                    if (currentBalance - amount < 0)
                        throw new BusinessRuleException("Saldo no disponible.");

                    var since = DateTime.UtcNow.Date;
                    var until = since.AddDays(1);

                    var retirosHoy = existingAccountEntity.Movements
                        .Where(m => m.AccountRefId == requestDto.AccountRefId
                                    && m.MoveType == MoveType.Debito
                                    && m.TransactionDate >= since
                                    && m.TransactionDate < until)
                        .Sum(m => m.Amount);

                    if (retirosHoy + amount > _dailyLimit)
                        throw new BusinessRuleException("Limite diario de retiros excedido.");
                }

                var newBalance = isDebit ? (currentBalance.Value - amount) : (currentBalance.Value + amount);

                var entity = _mapper.Map<Move>(requestDto);
                entity.Balance = newBalance;
                entity.Success = true;

                var insertedEntity = await _moveRepository.AddAsync(entity);
                return _mapper.Map<MoveResponseDto>(insertedEntity);
            }
            catch (Exception ex) when (ex is not NotFoundException)
            {
                try
                {
                    if (currentBalance is null && requestDto.AccountRefId != Guid.Empty)
                    {
                        var acc = (await _accountRepository.FindAsync(a => a.AccountId == requestDto.AccountRefId))
                                  .FirstOrDefault();
                        if (acc is not null)
                        {
                            currentBalance = acc.Movements
                                .OrderByDescending(m => m.TransactionDate)
                                .Select(m => m.Balance)
                                .FirstOrDefault(acc.InitialBalance);
                        }
                    }

                    var failed = _mapper.Map<Move>(requestDto);
                    failed.Balance = currentBalance ?? 0m;
                    failed.Success = false;
                    await _moveRepository.AddAsync(failed);
                }
                catch
                {
                    // Evita que un error al registrar el fallo o al recalcular el saldo
                    // tape la excepción original.
                }

                throw; 
            }
        }

        public async Task<IEnumerable<MoveResponseDto>> GetByAccountId(Guid accountId)
        {
            _ = (await _accountRepository.FindAsync(q => q.AccountId == accountId)).FirstOrDefault() ?? throw new NotFoundException("cuenta", accountId);
            var clientList = await _accountRepository.FindAsync(q => q.AccountId == accountId);
            return _mapper.Map<IEnumerable<MoveResponseDto>>(clientList);
        }
    }
}
