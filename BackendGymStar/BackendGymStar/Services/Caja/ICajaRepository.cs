using BackendGymStar.Data;
using BackendGymStar.DTO;
using BackendGymStar.DTO.Entradas;
using BackendGymStar.DTO.Salidas;

namespace BackendGymStar.Services.Caja
{
    public interface ICajaRepository
    {
        Task<Response<CajaDTO>> abrirCaja(AbrirCajaAdd cajaOpen);
        Task<CajaDTO> cajaById(long idCaja);
        Task<CajaDTO> cajaByIdGym(long indGym, long idUser);
        Task<Response<GeneralResp>> cerrarCaja(int idCaja);
        Task<Response<List<CajaDTO>>> historialCajaByGym(long idGym);
        Task<Response<List<DetallePago>>> pagoDetail(int pagoid);
        Task<Response<List<DetalleCaja>>> pagosBycaja(int cajaId);
        Task<Response<GeneralResp>> realizaPago(PagoAdd pagoAdd);
    }
}
