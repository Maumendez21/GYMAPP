using AutoMapper;
using BackendGymStar.Data;
using BackendGymStar.DTO.Salidas;

namespace BackendGymStar.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Membresia, MembresiaDTO>().ReverseMap();
            CreateMap<Caja, CajaDTO>().ReverseMap();
        }
    }
}
