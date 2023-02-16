using BackendGymStar.DTO;
using BackendGymStar.DTO.Entradas;
using BackendGymStar.DTO.Salidas;

namespace BackendGymStar.Services.Asocios
{
    public interface IAsocioService
    {
        Task<Response<MembresiaPagarGet>> membresiaAPagar(int idSocio, int idMemsocid);
        Task<Response<List<MembresiaAsociadas>>> membresiasAsociadas(long idGym);
        Task<Response<memsocidPago>> MembresiaUpdate(int memsocid);
        Task<Response<GeneralResp>> nuevoAsocio(AsocioAdd request);
        Task<Response<List<SociosList>>> sociosList(long idGym);
    }
}
