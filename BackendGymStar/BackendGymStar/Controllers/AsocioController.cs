using BackendGymStar.DTO.Entradas;
using BackendGymStar.Services.Asocios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendGymStar.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AsocioController : ControllerBase
    {
        private readonly IAsocioService _asocioService;

        public AsocioController(IAsocioService asocioService)
        {
            this._asocioService = asocioService;
        }

        [HttpPost("nuevoAsocio")]
        public async Task<ActionResult> nuevoAsocio([FromBody] AsocioAdd request)
        {
            var response = await _asocioService.nuevoAsocio(request);
            return Ok(response);
        }
        
        [HttpGet("listMembresiasAsocios/{idGym}")]
        public async Task<ActionResult> listMembresiasAsocios(long idGym)
        {
            var response = await _asocioService.membresiasAsociadas(idGym);
            return Ok(response);
        }
        
        [HttpGet("listSocios/{idGym}")]
        public async Task<ActionResult> listSocios(long idGym)
        {
            var response = await _asocioService.sociosList(idGym);
            return Ok(response);
        }
        
        [HttpGet("pagoMembresiaGet/{idSocio}/{idMemsocid}")]
        public async Task<ActionResult> pagoMembresiaGet(int idSocio, int idMemsocid)
        {
            var response = await _asocioService.membresiaAPagar(idSocio, idMemsocid);
            return Ok(response);
        }

        [HttpGet("pagoMembresiaPost/{memsocid}")]
        public async Task<ActionResult> pagoMembresiaPost(int memsocid)
        {
            var response = await _asocioService.MembresiaUpdate(memsocid);
            return Ok(response);
        }
    }
}
