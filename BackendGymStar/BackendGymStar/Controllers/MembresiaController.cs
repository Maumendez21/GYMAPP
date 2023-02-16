using BackendGymStar.DTO.General;
using BackendGymStar.DTO.Salidas;
using BackendGymStar.Services.Membresia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendGymStar.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MembresiaController : ControllerBase
    {
        private readonly IMembresiaRepository _membresiaRepository;

        public MembresiaController(IMembresiaRepository membresiaRepository)
        {
            this._membresiaRepository = membresiaRepository;
        }


        [HttpGet("listMemberships/{idGym}")]
        public async Task<ActionResult> getMembresias(long idGym)
        {
            var response = await _membresiaRepository.ListMembresias(idGym);
            return Ok(response);
        }
        
        [HttpPost("addMembership")]
        public async Task<ActionResult> addMembership([FromBody] MembresiaDTO membresia)
        {
            var response = await _membresiaRepository.ActionsMembership(membresia);
            return Ok(response);
        }
        
        
        [HttpPost("desHabDelMembership")]
        public async Task<ActionResult> desHabDelMembership([FromBody] DesHabDelMem model)
        {
            var response = await _membresiaRepository.desHabMembresia(model);
            return Ok(response);
        }







    }
}
