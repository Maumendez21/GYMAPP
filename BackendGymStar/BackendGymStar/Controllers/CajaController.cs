using BackendGymStar.DTO.Entradas;
using BackendGymStar.DTO.Salidas;
using BackendGymStar.Services.Caja;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendGymStar.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CajaController : ControllerBase
    {
        private readonly ICajaRepository _cajaRepository;

        public CajaController(ICajaRepository cajaRepository)
        {
            this._cajaRepository = cajaRepository;
        }


        [HttpPost("abrirCaja")]
        public async Task<ActionResult> addMembership([FromBody] AbrirCajaAdd caja)
        {
            var response = await _cajaRepository.abrirCaja(caja);
            return Ok(response);
        }
        
        
        [HttpGet("cajaByIdGym/{idGym}/{idUser}")]
        public async Task<ActionResult> cajaByIdGym(int idGym, int idUser)
        {
            var response = await _cajaRepository.cajaByIdGym(idGym, idUser);
            return Ok(response);
        }
        
        [HttpGet("cajaById/{idCaja}")]
        public async Task<ActionResult> cajaById(int idCaja)
        {
            var response = await _cajaRepository.cajaById(idCaja);
            return Ok(response);
        }
        
        
        [HttpGet("cajaPagos/{idCaja}")]
        public async Task<ActionResult> pagosBycaja(int idCaja)
        {
            var response = await _cajaRepository.pagosBycaja(idCaja);
            return Ok(response);
        }
        
        [HttpGet("cajaHistorial/{idGym}")]
        public async Task<ActionResult> cajaHistorial(long idGym)
        {
            var response = await _cajaRepository.historialCajaByGym(idGym);
            return Ok(response);
        }
        
        
        [HttpGet("cierraCaja/{idCaja}")]
        public async Task<ActionResult> cierraCaja(int idCaja)
        {
            var response = await _cajaRepository.cerrarCaja(idCaja);
            return Ok(response);
        }
        
        [HttpGet("cajaPagosDetail/{idPago}")]
        public async Task<ActionResult> pagoDetail(int idPago)
        {
            var response = await _cajaRepository.pagoDetail(idPago);
            return Ok(response);
        }


        [HttpPost("realizaPago")]
        public async Task<ActionResult> realizaPago([FromBody] PagoAdd pago)
        {
            var response = await _cajaRepository.realizaPago(pago);
            return Ok(response);
        }

    }
}
