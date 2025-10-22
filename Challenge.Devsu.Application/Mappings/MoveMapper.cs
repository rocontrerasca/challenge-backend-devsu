using AutoMapper;
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Core.Entities;

namespace Challenge.Devsu.Application.Mappings
{
    public class MoveMapper : Profile
    {
        public MoveMapper()
        {
            CreateMap<MoveDto, Move>()
              .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<Move, MoveDto>();
            CreateMap<MoveResponseDto, Move>().ReverseMap();
        }
    }
}
