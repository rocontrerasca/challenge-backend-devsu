using AutoMapper;
using Challenge.Devsu.Application.DTOs;
using Challenge.Devsu.Core.Entities;

namespace Challenge.Devsu.Application.Mappings
{
    public class ClientMapper : Profile
    {
        public ClientMapper()
        {
            CreateMap<ClientDto, Client>()
              .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
              .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
              .ForMember(dest => dest.Active, opt => opt.MapFrom(_ => true));
            CreateMap<Client, ClientDto>();
            CreateMap<ClientUpdateDto, Client>()
              .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<Client, ClientResponseDto>();
            CreateMap<Client, ClientUpdateDto>();
        }
    }
}
