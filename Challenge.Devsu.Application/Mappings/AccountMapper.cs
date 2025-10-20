using AutoMapper;
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Core.Entities;

namespace Challenge.Devsu.Application.Mappings
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<AccountDto, Account>()
              .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
              .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<AccountUpdateDto, Account>()
             .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<Account, AccountResponseDto>().ReverseMap();
        }
    }
}
