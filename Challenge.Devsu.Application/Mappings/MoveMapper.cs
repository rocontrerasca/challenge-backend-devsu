using AutoMapper;
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Core.Entities;

namespace Challenge.Devsu.Application.Mappings
{
    public class MoveMapper : Profile
    {
        public MoveMapper()
        {
            CreateMap<MoveDto, Move>().ReverseMap();
            CreateMap<MoveResponseDto, Move>();
        }
    }
}
