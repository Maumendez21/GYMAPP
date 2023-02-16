using BackendGymStar.DTO;
using BackendGymStar.DTO.General;
using BackendGymStar.DTO.Salidas;

namespace BackendGymStar.Services.Membresia
{
    public interface IMembresiaRepository
    {
        Task<Response<MembresiaDTO>> ActionsMembership(MembresiaDTO membresiaRequest);
        Task<Response<SalidasResponse>> desHabMembresia(DesHabDelMem model);
        Task<Response<List<MembresiaDTO>>> ListMembresias(long idGym);
    }
}
